using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Models.Identity;
using PayrollSystem.ApiService.Requests;
using PayrollSystem.ApiService.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PayrollSystem.ApiService.Services;

public interface IUserService
{
    Task<Results<Ok<LoginResponse>, BadRequest<ApiResponse>>> Login(LoginRequest request);
    Task<Results<Ok, BadRequest<List<IdentityError>>>> Register(RegisterRequest request);
}
public class UserService(
    UserManager<User> userManager,
    IConfiguration configuration
    ) : IUserService
{
    public async Task<Results<Ok<LoginResponse>, BadRequest<ApiResponse>>> Login(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.UsernameOrEmail);
        if (user == null)
            user = await userManager.FindByNameAsync(request.UsernameOrEmail);

        if(user == null)
            return TypedResults.BadRequest(new ApiResponse { Message = "User not found." });

        if(!await userManager.CheckPasswordAsync(user, request.Password))
            return TypedResults.BadRequest(new ApiResponse { Message = "Invalid Password." });

        var token = GenerateJwtToken(user, configuration);
        return TypedResults.Ok(new LoginResponse
        {
            Token = await token
        });

    }

    public async Task<Results<Ok, BadRequest<List<IdentityError>>>> Register(RegisterRequest request)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return TypedResults.BadRequest(new List<IdentityError> { new() { Description = "User with this email already exists." } });
        existingUser = await userManager.FindByNameAsync(request.Username);
        if (existingUser != null)
            return TypedResults.BadRequest(new List<IdentityError> { new() { Description = "User with this username already exists." } });

        var user = new User
        {
            UserName = request.Username,
            Email = request.Email,
            TwoFactorEnabled = false
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Employee");
            return TypedResults.Ok();
        }

        return TypedResults.BadRequest(result.Errors.ToList());
    }

    private async Task<string> GenerateJwtToken(User user, IConfiguration config)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
        };

        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}