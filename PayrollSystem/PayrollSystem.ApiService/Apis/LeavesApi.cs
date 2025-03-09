using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Helper;
using PayrollSystem.ApiService.Requests.Employee;
using PayrollSystem.ApiService.Requests.Leave;
using PayrollSystem.ApiService.Responses;
using PayrollSystem.ApiService.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;

namespace PayrollSystem.ApiService.Apis;

public static class LeavesApi
{
    public static RouteGroupBuilder MapLeavesApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/leaves");

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

    private static async Task<Results<Ok<LeaveResponse>, NotFound>> Get(
        int id,
        HttpRequest httpRequest,
        [FromServices] ILeaveService service
    )
    {
        var employeeId = int.Parse(httpRequest.GetClaim(AppConstants.JWT_EMPLOYEE));
        var result = await service.Get(id, employeeId);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }


    private static async Task<Results<Ok<bool?>, NotFound>> Insert(
        HttpRequest httpRequest,
        [FromBody] CreateLeaveRequest request,
        [FromServices] ILeaveService service)
    {
        var employeeId = int.Parse(httpRequest.GetClaim(AppConstants.JWT_EMPLOYEE));
        request.EmployeeId = employeeId;
        var result = await service.Insert(request);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<EmployeeResponse>, NotFound>> Update(
        HttpRequest httpRequest,
        [FromBody] UpdateLeaveRequest request,
        [FromServices] ILeaveService service
    )
    {
        var employeeId = int.Parse(httpRequest.GetClaim(AppConstants.JWT_EMPLOYEE));
        request.EmployeeId = employeeId;
        //var userId = httpRequest.GetClaim(JwtRegisteredClaimNames.Sub);
        //var result = await service.Update(userId, request);
        //if (result == null)
        //    return TypedResults.NotFound();
        //return TypedResults.Ok(result);

        throw new NotImplementedException();
    }

    private static async Task<Results<Ok<ListResponse<LeaveResponse>>, NotFound>> List(
        HttpRequest httpRequest,
        LeaveListRequest request,
        [FromServices] ILeaveService service
    )
    {
        var employeeId = int.Parse(httpRequest.GetClaim(AppConstants.JWT_EMPLOYEE));
        request.EmployeeId = employeeId;
        var result = await service.List(request);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<bool>, NotFound>> Delete(HttpContext context)
    {
        throw new NotImplementedException();
    }

}