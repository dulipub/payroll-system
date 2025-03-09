using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollSystem.ApiService.Models;

public class BankAccount : BaseModel
{
    public string? AccountNo { get; set; }
    public string? AccountName { get; set; }
    public string? Bank { get; set; }
    public string? Branch { get; set; }
    public required int EmployeeId { get; set; }

}
