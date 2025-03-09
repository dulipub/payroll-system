namespace PayrollSystem.ApiService.Requests.TimeSheet;

public class TimeSheetListRequest : ListRequest
{
    public int EmployeeId { get; set; }
}
