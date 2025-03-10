
namespace PayrollSystem.ApiService.Models;

public class Employee : BaseModel
{
    public required string Address { get; set; }
    public double BasicSalary { get; set; }
    public double HourlyRate { get; set; }
    public required string Name { get; set; }
    public required string NIC { get; set; }
    public string? UserId { get; set; }

    public List<BankAccount>? BankAccounts { get; set; }
    public List<EmployeePayAdjustment>? PayAdjustments { get; set; }
}