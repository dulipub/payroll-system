namespace PayrollSystem.ApiService.Models;

public class Project : BaseModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
}
