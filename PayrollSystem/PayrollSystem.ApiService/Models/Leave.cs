namespace PayrollSystem.ApiService.Models;

public class Leave : BaseModel
{
    public required DateTime Date { get; set; }
    public bool IsApproved { get; set; }
    public string? ApprovedUserId { get; set; }

    public required int EmployeeId { get; set; }

    public Employee? Employee   { get; set; }
}
