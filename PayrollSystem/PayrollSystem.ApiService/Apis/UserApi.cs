using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using PayrollSystem.ApiService.Models.Identity;
using PayrollSystem.ApiService.Requests;
using PayrollSystem.ApiService.Responses;
using PayrollSystem.ApiService.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PayrollSystem.ApiService.Apis;

public static class UserApi
{

    public static RouteGroupBuilder MapUserApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/user");

        api.MapPost("/register", Register).AllowAnonymous();
        api.MapPost("/login", Login).AllowAnonymous();

        return api
            .RequireRateLimiting("PerDeviceSliding")
            .WithOpenApi();
    }

    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    private static async Task<Results<Ok<LoginResponse>, BadRequest<ApiResponse>>> Login(
    [FromServices] IUserService service,
    [FromBody] LoginRequest request
)
    {
        return await service.Login(request);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<IdentityError>), StatusCodes.Status400BadRequest)]
    private static async Task<Results<Ok, BadRequest<List<IdentityError>>>> Register(
    [FromServices] IUserService service,
    [FromBody] RegisterRequest request
)
    {
        return await service.Register(request);
    }
}
