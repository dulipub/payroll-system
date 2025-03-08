namespace PayrollSystem.ApiService.Requests.Employee;

public class InsertEmployeeRequest
{
    public required string Name { get; set; }
    public required string NIC { get; set; }
    public required string Address { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal HourlyRate { get; set; }

    //create user
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}