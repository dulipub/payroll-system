namespace PayrollSystem.ApiService.Requests;
public sealed class LoginRequest
{
    public required string UsernameOrEmail { get; init; }
    public required string Password { get; init; }
}