using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Helper;
using PayrollSystem.ApiService.Requests.Employee;
using PayrollSystem.ApiService.Requests.PayAdjustment;
using PayrollSystem.ApiService.Requests.TimeSheet;
using PayrollSystem.ApiService.Responses;
using PayrollSystem.ApiService.Services;
using System.IdentityModel.Tokens.Jwt;

namespace PayrollSystem.ApiService.Apis;

public static class PayAdjustmentApi
{
    public static RouteGroupBuilder MapPayAdjustmentApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/payadjustment");

        api.MapGet("/{id}", Get);

        api.MapPost("/", Insert);
        api.MapPost("/update", Update);
        api.MapPost("/list", List);
        //add to employees

        api.MapDelete("/{id}", Delete);

        return api.RequireAuthorization(policy => policy.RequireRole("Admin", "HumanResources")).WithOpenApi();
    }

    private static async Task<Results<Ok<PayAdjustmentResponse>, NotFound>> Get(
        int id,
        HttpRequest httpRequest,
        [FromServices] IPayAdjustmentService service
    )
    {
        var employeeId = int.Parse(httpRequest.GetClaim(AppConstants.JWT_EMPLOYEE));
        var result = await service.Get(id, employeeId);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<bool>, BadRequest>> Insert(
        [FromBody] CreatePayAdjustmentRequest request,
        [FromServices] IPayAdjustmentService service,
        CancellationToken token
    )
    {
        var result = await service.Insert(request);
        if (result == false)
            return TypedResults.BadRequest();
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<bool>, BadRequest>> Update(
        [FromBody] UpdatePayAdjustmentRequest request,
        [FromServices] IPayAdjustmentService service,
        CancellationToken token
    )
    {
        var result = await service.Update(request);
        if (result == false)
            return TypedResults.BadRequest();
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<ListResponse<PayAdjustmentResponse>>, NotFound>> List(
        PayAdjustmentListRequest request,
        [FromServices] IPayAdjustmentService service
    )
    {
        var result = await service.List(request);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    private static async Task Delete(HttpContext context)
    {
        throw new NotImplementedException();
    }
}