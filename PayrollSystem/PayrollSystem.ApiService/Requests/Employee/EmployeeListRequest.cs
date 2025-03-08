namespace PayrollSystem.ApiService.Requests.Employee;

public class EmployeeListRequest : ListRequest
{
    public int? ProjectId { get; set; } = 0;
    public int? DepartmentId { get; set; } = 0;

    public bool IsActive { get; set; } = true;
}