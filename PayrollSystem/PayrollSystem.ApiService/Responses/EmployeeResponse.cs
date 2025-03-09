namespace PayrollSystem.ApiService.Responses;

public sealed class EmployeeResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? NIC { get; set; }

    public decimal BasicSalary { get; set; }
    public decimal HourlyRate { get; set; }
}