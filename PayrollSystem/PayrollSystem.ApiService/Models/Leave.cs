using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollSystem.ApiService.Models;

public class Leave : BaseModel
{
    public required DateOnly Date { get; set; }
    public bool IsApproved { get; set; }
    public string? ApprovedUserId { get; set; }
    public double LeaveDuration { get; set; }

    public required int EmployeeId { get; set; }

    [ForeignKey("EmployeeId")]
    public Employee? Employee   { get; set; }

    public int LeaveTypeId { get; set; }

    [ForeignKey("LeaveTypeId")]
    public LeaveType? LeaveType { get; set; }
}


public class LeaveType : BaseModel
{
    public required string Type { get; set; }
    public bool IsPaid { get; set; }
    public float MaximumAllowedDays { get; set; }
}