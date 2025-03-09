namespace PayrollSystem.ApiService.Models;

public class PayAdjustment : BaseModel
{
    public string? Name { get; set; }
    public PayAdjustmentType Type { get; set; } 
    public bool Recurring { get; set; }
}

public enum PayAdjustmentType
{
    Allowance,
    Deduction
}

public class EmployeePayAdjustment : BaseModel
{
    public string? Details { get; set; }
    public float Amount { get; set; }
    public int EmployeeId { get; set; }
    public int PayAdjustmentId { get; set; }
}
