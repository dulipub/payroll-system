namespace PayrollSystem.ApiService.Models;

public class Paysheet : BaseModel
{
    public decimal GrossPay { get; set; }
    public decimal NetPay { get; set; }
    public decimal EPF_EmployeePay { get; set; }
    public decimal EPF_EmployerPay { get; set; }
    public decimal ETF { get; set; }
}
