namespace PayrollSystem.ApiService.Responses;

public class LeaveResponse
{
    public int Id { get; set; }
    public required DateTime Date { get; set; }
    public bool IsApproved { get; set; }
    public string? ApprovedUserId { get; set; }
    public int LeaveTypeId { get; set; }
    public bool IsPaid { get; set; }
    public float DurationInDays { get; set; }

    public required int EmployeeId { get; set; }
}