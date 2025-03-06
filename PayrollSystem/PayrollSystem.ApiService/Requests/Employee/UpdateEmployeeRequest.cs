namespace PayrollSystem.ApiService.Requests.Employee;

public class UpdateEmployeeRequest
{
    public required string Name { get; set; }
    public required string? NIC { get; set; }
    public required string Address { get; set; }
    public string? Type { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal HourlyRate { get; set; }
}