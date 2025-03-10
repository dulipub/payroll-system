namespace PayrollSystem.ApiService.Models;

public class PayAdjustmentRecord : BaseModel
{
    public string? Description { get; set; }
    public double Amount { get; set; }

    public int PaySheetId { get; set; }
    public int EmployeeId { get; set; }
}