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
