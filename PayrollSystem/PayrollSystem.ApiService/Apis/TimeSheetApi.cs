using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Requests.Employee;
using PayrollSystem.ApiService.Responses;
using PayrollSystem.ApiService.Services;
using System.IdentityModel.Tokens.Jwt;

namespace PayrollSystem.ApiService.Apis;

public static class TimeSheetApi
{
    public static RouteGroupBuilder MapTimeSheetApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/timesheet");

        api.MapGet("/{id}", Get);

        api.MapPost("/", Insert);
        api.MapPost("/update", Update);
        api.MapPost("/list", List);

        api.MapDelete("/{id}", Delete);

        //administration
        //approve list
        //approve

        return api.RequireAuthorization().WithOpenApi();
    }

    private static async Task List(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task Delete(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task Insert(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task Get(HttpContext context)
    {
        throw new NotImplementedException();
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