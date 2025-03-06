using Microsoft.AspNetCore.Identity;
using PayrollSystem.ApiService.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollSystem.ApiService.Models;

public class Employee : BaseModel
{
    public required string Address { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal HourlyRate { get; set; }
    public required string Name { get; set; }
    public required string? NIC { get; set; }
    public string? Type { get; set; }

    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } // Navigation property

    public List<BankAccount>? BankAccounts { get; set; }
}