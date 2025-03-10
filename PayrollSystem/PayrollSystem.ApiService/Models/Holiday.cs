namespace PayrollSystem.ApiService.Models;

public class Holiday : BaseModel
{
    public DateOnly Date { get; set; }
    public bool IsApproved { get; set; }
    public string? Description { get; set; }
}
