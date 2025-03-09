namespace PayrollSystem.ApiService.Responses;

public class TimeSheetResponse
{
    public DateTime Date { get; set; }
    public string? Detail { get; set; }
    public decimal HoursWorked { get; set; }
    public bool IsApproved { get; set; }
}