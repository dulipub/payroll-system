using PayrollSystem.ApiService.Models;

namespace PayrollSystem.ApiService.Responses;

public class PayAdjustmentResponse
{
    public int Id { get; set; }
    public string? Details { get; set; }
    public DateTime Date { get; set; }
    public PayAdjustmentType Type { get; set; }
    public bool Recurring { get; set; }
}