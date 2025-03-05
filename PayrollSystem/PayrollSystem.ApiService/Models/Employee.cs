using Microsoft.AspNetCore.Identity;

namespace PayrollSystem.ApiService.Models;

public class Employee : BaseModel
{
    public required string Name { get; set; }
    public required string? NIC { get; set; }
    public required string Address { get; set; }
    public string? Type { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal HourlyRate { get; set; }

    public List<BankAccount>? BankAccounts { get; set; }
}