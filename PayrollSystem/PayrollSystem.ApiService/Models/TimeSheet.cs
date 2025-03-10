namespace PayrollSystem.ApiService.Models;

public class TimeSheet : BaseModel
{
    public int EmployeeId { get; set; }
    public DateOnly Date { get; set; }
    public string? Detail { get; set; }
    public double HoursWorked { get; set; }
    public bool IsApproved { get; set; }
    public string? ApprovedUserId { get; set; }
}
