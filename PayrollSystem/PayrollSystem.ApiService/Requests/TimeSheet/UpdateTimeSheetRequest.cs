namespace PayrollSystem.ApiService.Requests.TimeSheet;

public class UpdateTimeSheetRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public string? Detail { get; set; }
    public decimal HoursWorked { get; set; }
    public bool IsApproved { get; set; }
    public string? ApprovedUserId { get; set; }
}