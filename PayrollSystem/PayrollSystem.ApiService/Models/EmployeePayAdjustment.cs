using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollSystem.ApiService.Models;

public class EmployeePayAdjustment : BaseModel
{
    public string? Description { get; set; }
    public float Amount { get; set; }
    public int EmployeeId { get; set; }

    public int PayAdjustmentId { get; set; }
    [ForeignKey("PayAdjustmentId")]
    public PayAdjustment? PayAdjustment { get; set; }
}
