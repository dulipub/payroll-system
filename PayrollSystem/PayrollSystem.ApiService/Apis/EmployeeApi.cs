using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Requests.Employee;
using PayrollSystem.ApiService.Responses;
using PayrollSystem.ApiService.Services;
using System.IdentityModel.Tokens.Jwt;

namespace PayrollSystem.ApiService.Apis;

public static class EmployeeApi
{
    public static RouteGroupBuilder MapEmployeeApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/employee");

        api.MapGet("/profile", Get);
        api.MapPost("/update", Update);

        return api.RequireAuthorization().WithOpenApi();
    }


    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private static async Task<Results<Ok<EmployeeResponse>, NotFound>> Get(
        HttpRequest httpRequest,
        [FromServices] IEmployeeService service
    )
    {
        var userId = httpRequest.GetClaim(JwtRegisteredClaimNames.Sub);
        var result = await service.GetByUserId(userId);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<EmployeeResponse>, NotFound>> Update(
        HttpRequest httpRequest,
        [FromBody] UpdateEmployeeRequest request,
        [FromServices] IEmployeeService service,
        CancellationToken token
    )
    {
        var userId = httpRequest.GetClaim(JwtRegisteredClaimNames.Sub);
        var result = await service.Update(userId, request);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }
}