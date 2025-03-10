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

            await CalculatePay(payroll, employee);
        }
    }

    private async Task CalculatePay(Payroll payroll, Employee employee)
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

        await CalculateAdjustments(payroll, paysheet);
        CalculateEPF(payroll, paysheet);
        CalculateTax(payroll, paysheet);
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

    private async Task CalculateAdjustments(Payroll payroll, Paysheet paysheet)
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

    private void CalculateEPF(Payroll payroll, Paysheet paysheet)
    {
        throw new NotImplementedException();
    }

    private void CalculateTax(Payroll payroll, Paysheet paysheet)
    {
        throw new NotImplementedException();
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