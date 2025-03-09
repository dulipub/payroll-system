using PayrollSystem.ApiService.Models;

namespace PayrollSystem.ApiService.Requests.PayAdjustment;

public class UpdatePayAdjustmentRequest
{
    public int Id { get; set; }
    public string? Details { get; set; }
    public PayAdjustmentType Type { get; set; }
    public bool Recurring { get; set; }
}
