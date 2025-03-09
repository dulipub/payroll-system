namespace PayrollSystem.ApiService.Models;

public class Leave : BaseModel
{
    public required DateTime Date { get; set; }
    public bool IsApproved { get; set; }
    public string? ApprovedUserId { get; set; }
    public int LeaveTypeId { get; set; }
    public float LeaveDuration { get; set; }

    public required int EmployeeId { get; set; }
    public Employee? Employee   { get; set; }
}


public class LeaveType : BaseModel
{
    public required string Type { get; set; }
    public bool IsPaid { get; set; }
    public float MaximumAllowedDays { get; set; }
}