namespace PayrollSystem.ApiService.Requests.Employee;

public class InsertEmployeeRequest
{
    public required string Name { get; set; }
    public required string NIC { get; set; }
    public required string Address { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal HourlyRate { get; set; }
    public string? UserId { get; set; }
    public UserDto? User { get; set; }

    public BankAccountDto? BankAccount { get; set; }

}

public class UserDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class BankAccountDto
{
    public string? AccountNo { get; set; }
    public string? AccountName { get; set; }
    public string? Bank { get; set; }
    public string? Branch { get; set; }
}