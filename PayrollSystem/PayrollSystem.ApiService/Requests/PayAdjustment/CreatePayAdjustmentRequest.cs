using PayrollSystem.ApiService.Models;

namespace PayrollSystem.ApiService.Requests.PayAdjustment;

public class CreatePayAdjustmentRequest
{
    public string? Details { get; set; }
    public PayAdjustmentType Type { get; set; }
    public bool Recurring { get; set; }
}

public class PayAdjustmentListRequest : ListRequest
{
}