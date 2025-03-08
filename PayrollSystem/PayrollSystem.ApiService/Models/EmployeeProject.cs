namespace PayrollSystem.ApiService.Models;

public class EmployeeProject : BaseModel
{
    public int ProjectId { get; set; }
    public int EmployeeId { get; set; }

    public Project? Project { get; set; }
    public Employee? Employee { get; set; }
}