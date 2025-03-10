using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Helper;
using PayrollSystem.ApiService.Requests.PayAdjustment;
using PayrollSystem.ApiService.Responses;
using PayrollSystem.ApiService.Services;

namespace PayrollSystem.ApiService.Apis;

public static class PayAdjustmentApi
{
    public static RouteGroupBuilder MapPayAdjustmentApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/payadjustment");

        api.MapGet("/{id}", Get);

        api.MapPost("/", Insert);
        api.MapPost("/list", List);
        api.MapPost("/addemployees", AddPayAdjustmentToEmployees);

        api.MapDelete("/{id}", Delete);

        return api.RequireAuthorization(policy => policy.RequireRole("Admin", "HumanResources")).WithOpenApi();
    }

    private static async Task<Results<Ok<PayAdjustmentResponse>, NotFound>> Get(
        int id,
        [FromServices] IPayAdjustmentService service
    )
    {
        var result = await service.Get(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<bool>, BadRequest>> Insert(
        [FromBody] CreatePayAdjustmentRequest request,
        [FromServices] IPayAdjustmentService service
    )
    {
        var result = await service.Insert(request);
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

    private static async Task<Results<Ok<bool>, BadRequest>> AddPayAdjustmentToEmployees(
        AddPayAdjustmentToEmployeesRequest request,
        [FromServices] IEmployeePayAdjustmentService service)
    {
        var result = await service.AddPayAdjustmentToEmployees(request);
        if (result == false)
            return TypedResults.BadRequest();
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<bool>, BadRequest>> Delete(
        int id,
        [FromServices] IPayAdjustmentService service)
    {
        var result = await service.Delete(id);
        if (result == false)
            return TypedResults.BadRequest();
        return TypedResults.Ok(result);
    }
}