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

public class AddPayAdjustmentToEmployeesRequest
{
    public int PayAdjustmentId { get; set; }
    public List<int>? EmployeeIds { get; set; }
    public bool AddToAllEmployees { get; set; } = false;
    public float Amount { get; set; }
}

public class UpdatePayAdjustmentRequest
{
    public required int Id { get; set; }
    public string? Details { get; set; }
    public bool Recurring { get; set; }
}