namespace PayrollSystem.ApiService.Models;

public class Paysheet : BaseModel
{
    public double BasicSalary { get; set; }
    public double OverTimePay { get; set; }
    public double TotalAllowances { get; set; }
    public double GrossPay { get; set; }
    public double TotalDeductions { get; set; }
    public double EPF_EmployeePay { get; set; }
    public double EPF_EmployerPay { get; set; }
    public double ETF { get; set; }
    public double NetPay { get; set; }


    public int PayrollId { get; set; }
    public int EmployeeId { get; set; }

    public List<PaysheetRecord>? Records { get; set; }
    public List<TaxRecord>? TaxRecords { get; set; }
}
