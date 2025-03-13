using PayrollSystem.ApiService.Helper;
using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Repositories;

namespace PayrollSystem.ApiService.Services;

public interface IPayrollService
{
    Task CalculatePayroll(CalculatePayrollRequest request);
}

public class PayrollService(
    IRepository<Payroll> payrollRepository,
    IRepository<Paysheet> paysheetRepository,
    IRepository<TimeSheet> timesheetRepository,
    IRepository<Leave> leaveRepository,
    IRepository<Holiday> holidayRepository,
    IRepository<Tax> taxRepository,
    IRepository<EmployeePayAdjustment> employeePayAdjustmentRepository,
    IEmployeeRepository employeeRepository) : IPayrollService
{
    public async Task CalculatePayroll(CalculatePayrollRequest request)
    {
        var payroll = new Payroll
        {
            Year = DateTime.Now.Year,
            Month = DateTime.Now.Month,
            IsActive = true,
            DeductNoPay = request.DeductNoPay,
            AllowOT = request.AllowOT
        };
        await payrollRepository.Add(payroll);

        foreach(var id in request.EmployeeIds ?? [])
        {
            var employee = await employeeRepository.GetById(id);
            if (employee == null)
                continue;

            var paysheet = await CalculatePay(payroll, employee);
            await paysheetRepository.Add(paysheet);
        }
    }

    private async Task<Paysheet> CalculatePay(Payroll payroll, Employee employee)
    {
        var paysheet = new Paysheet
        {
            Id = payroll.Id,
            EmployeeId = employee.Id,
            BasicSalary = employee.BasicSalary,
            Records = new List<PaysheetRecord>()
        };
        var monthlyTimeSheet = new MonthlyTimeSheet { EmployeeId = employee.Id};

        await CalculateTimeSheets(payroll, paysheet, monthlyTimeSheet);
        await CheckLeaves(payroll, paysheet, monthlyTimeSheet);

        CalculateWorkHourAdjustments(employee, payroll, paysheet, monthlyTimeSheet);

        await CalculateAdjustments(paysheet);
        await CalculateGrossSalary(paysheet);
        await CalculateEPF(paysheet);
        await CalculateTax(paysheet);

        paysheet.NetPay = paysheet.GrossPay - paysheet.EPF_EmployeePay - paysheet.TaxRecords!.Sum(x => x.Amount);
        return paysheet;
    }

    private async Task CalculateTimeSheets(Payroll payroll, Paysheet paysheet, MonthlyTimeSheet monthlyTimeSheet)
    {
        var startDate = new DateOnly(payroll.Year, payroll.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var timesheets = await timesheetRepository.ListAscending(
            x => x.Date >= startDate && x.Date <= endDate && x.EmployeeId == paysheet.EmployeeId,
            x => x.Id);
        var holidays = await holidayRepository.ListAscending(x => x.Date >= startDate && x.Date <= endDate, x=> x.Id);

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || holidays.Any(h => h.Date == date))
            {
                // Check if there is OT timesheet
                var otEntry = timesheets.FirstOrDefault(t => t.Date == date);
                if (otEntry != null && otEntry.IsApproved)
                {
                    monthlyTimeSheet.EmployeeWorkingHours += otEntry.HoursWorked;
                    monthlyTimeSheet.OverTimeHours += otEntry.HoursWorked;
                }
                continue;
            }

            // Check if there is a timesheet entry for the current date
            var timesheetEntry = timesheets.FirstOrDefault(t => t.Date == date);
            if (timesheetEntry != null)
            {
                monthlyTimeSheet.EmployeeWorkingHours += timesheetEntry.HoursWorked;
                monthlyTimeSheet.OverTimeHours += (timesheetEntry.HoursWorked - AppConstants.WORKING_HOURS_PER_DAY);
            }
            monthlyTimeSheet.MonthlyWorkingHours += AppConstants.WORKING_HOURS_PER_DAY;
        }
    }

    private async Task CheckLeaves(Payroll payroll, Paysheet paysheet, MonthlyTimeSheet monthlyTimeSheet)
    {
        var startDate = new DateOnly(payroll.Year, payroll.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        var leaves = await leaveRepository.ListAscending(
            x => x.Date >= startDate && x.Date <= endDate && x.EmployeeId == paysheet.EmployeeId,
            x => x.Id,
            "LeaveType");
        foreach (var leave in leaves)
        {
            if (!leave.IsActive)
            {
                continue;
            }
            else if (leave.IsApproved && (leave.LeaveType?.IsPaid ?? false)) //if leave is not approved, it is not paid
            {
                monthlyTimeSheet.PaidLeaves += (AppConstants.WORKING_HOURS_PER_DAY * leave.LeaveDuration);
            }
            else
            {
                monthlyTimeSheet.UnPadiLeaves += (AppConstants.WORKING_HOURS_PER_DAY * leave.LeaveDuration);
            }
        }
    }

    private void CalculateWorkHourAdjustments(Employee employee, Payroll payroll, Paysheet paysheet, MonthlyTimeSheet monthlyTimeSheet)
    {
        if (payroll.AllowOT)
        {
            var otRecord = new PaysheetRecord
            {
                PaysheetId = paysheet.Id,
                Description = "Overtime Pay",
                Amount = monthlyTimeSheet.OverTimeHours * employee.HourlyRate
            };

            paysheet.Records?.Add(otRecord);
            paysheet.OverTimePay = otRecord.Amount;
        }

        if (payroll.DeductNoPay)
        {
            var unpaidRecord = new PaysheetRecord
            {
                PaysheetId = paysheet.Id,
                Description = "Unpaid Leave Deduction",
                Amount = monthlyTimeSheet.UnPadiLeaves * employee.HourlyRate * -1 //negative value
            };
            paysheet.Records?.Add(unpaidRecord);
            paysheet.TotalDeductions += unpaidRecord.Amount;
        }

        if(payroll.DeductNoPay && (monthlyTimeSheet.EmployeeWorkingHours - monthlyTimeSheet.MonthlyWorkingHours) < 0)
        {
            var missingWorkHoursRecord = new PaysheetRecord
            {
                PaysheetId = paysheet.Id,
                Description = "Missing Work Hours",
                Amount = (monthlyTimeSheet.EmployeeWorkingHours - monthlyTimeSheet.MonthlyWorkingHours) * employee.HourlyRate //negative value
            };
            paysheet.Records?.Add(missingWorkHoursRecord);
            paysheet.TotalDeductions += missingWorkHoursRecord.Amount;
        }
    }

    private async Task CalculateAdjustments(Paysheet paysheet)
    {
        var adjustments = await employeePayAdjustmentRepository.ListAscending(x => x.EmployeeId == paysheet.EmployeeId, x => x.Id, "PayAdjustment");
        foreach(var adjustment in adjustments)
        {
            if (!adjustment.IsActive)
            {
                continue;
            }
            var adjustmentRecord = new PaysheetRecord
            {
                PaysheetId = paysheet.Id,
                Description = adjustment.Description,
                Amount = adjustment.Amount
            };
            paysheet.Records?.Add(adjustmentRecord);

            if (adjustment.PayAdjustment?.Type == PayAdjustmentType.Allowance)
            {
                paysheet.TotalAllowances += adjustmentRecord.Amount;
            }
            else
            {
                paysheet.TotalDeductions += adjustmentRecord.Amount;
            }
        }
    }

    private async Task CalculateGrossSalary(Paysheet paysheet)
    {
        paysheet.GrossPay = paysheet.BasicSalary + paysheet.TotalAllowances + paysheet.OverTimePay;
    }

    private async Task CalculateEPF(Paysheet paysheet)
    {
        paysheet.EPF_EmployeePay = paysheet.BasicSalary * AppConstants.EPF_EMPLOYEE_RATE;
        paysheet.EPF_EmployerPay = paysheet.BasicSalary * AppConstants.EPF_EMPLOYERS_RATE;
    }

    private async Task CalculateTax(Paysheet paysheet)
    {
        var taxBrackets = await taxRepository.ListAscending(x => x.IsActive && x.Type == "APIT");
        paysheet.TaxRecords = new List<TaxRecord>();

        foreach (var bracket in taxBrackets)
        {
            if (paysheet.GrossPay >= bracket.StartAmount && paysheet.GrossPay <= bracket.EndAmount)
            {
                double taxAmount = (paysheet.GrossPay - bracket.StartAmount) * bracket.Percentage;
                paysheet.TaxRecords.Add(new TaxRecord { 
                    Amount = taxAmount, 
                    Description = $"Tax Bracket: {bracket.StartAmount}-{bracket.EndAmount}",,
                    StartAmount = bracket.StartAmount,
                    EndAmount = bracket.EndAmount,
                    TaxId = bracket.Id,
                    PaysheetId = paysheet.Id
                });
                break; 
            }
            else if (paysheet.GrossPay >= bracket.StartAmount && paysheet.GrossPay > bracket.EndAmount)
            {
                double taxAmount = (bracket.EndAmount - bracket.StartAmount) * bracket.Percentage;
                paysheet.TaxRecords.Add(new TaxRecord
                {
                    Amount = taxAmount,
                    Description = $"Tax Bracket: {bracket.StartAmount}-{bracket.EndAmount}",
                    StartAmount = bracket.StartAmount,
                    EndAmount = bracket.EndAmount,
                    TaxId = bracket.Id,
                    PaysheetId = paysheet.Id
                });
                break;
            }
        }
    }
}

public class CalculatePayrollRequest
{
    public bool AllowOT { get; set; }
    public bool DeductNoPay { get; set; }
    public int[]? EmployeeIds { get; set; }
}

public class MonthlyTimeSheet
{
    public int EmployeeId { get; set; }
    public double MonthlyWorkingHours { get; set; }
    public double EmployeeWorkingHours { get; set; }
    public double OverTimeHours { get; set; }
    public double PaidLeaves { get; set; }
    public double UnPadiLeaves { get; set; }
}