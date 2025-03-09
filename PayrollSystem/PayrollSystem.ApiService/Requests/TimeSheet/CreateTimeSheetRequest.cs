namespace PayrollSystem.ApiService.Requests.TimeSheet;

public class CreateTimeSheetRequest
{
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public string? Detail { get; set; }
    public decimal HoursWorked { get; set; }
}