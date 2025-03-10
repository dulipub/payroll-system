﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Helper;
using PayrollSystem.ApiService.Requests.Employee;
using PayrollSystem.ApiService.Requests.TimeSheet;
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

    private static async Task<Results<Ok<TimeSheetResponse>, NotFound>> Get(
        int id,
        HttpRequest httpRequest,
        [FromServices] ITimeSheetService service
    )
    {
        var employeeId = int.Parse(httpRequest.GetClaim(AppConstants.JWT_EMPLOYEE));
        var result = await service.Get(id, employeeId);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<bool>, BadRequest>> Insert(
        HttpRequest httpRequest,
        [FromBody] CreateTimeSheetRequest request,
        [FromServices] ITimeSheetService service,
        CancellationToken token
    )
    {
        var employeeId = int.Parse(httpRequest.GetClaim(AppConstants.JWT_EMPLOYEE));
        request.EmployeeId = employeeId;
        var result = await service.Insert(request);
        if (result == false)
            return TypedResults.BadRequest();
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<bool>, BadRequest>> Update(
        HttpRequest httpRequest,
        [FromBody] UpdateTimeSheetRequest request,
        [FromServices] ITimeSheetService service,
        CancellationToken token
    )
    {
        var employeeId = int.Parse(httpRequest.GetClaim(AppConstants.JWT_EMPLOYEE));
        request.EmployeeId = employeeId;
        var result = await service.Update(request);
        if (result == false)
            return TypedResults.BadRequest();
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<ListResponse<TimeSheetResponse>>, NotFound>> List(
        HttpRequest httpRequest,
        TimeSheetListRequest request,
        [FromServices] ITimeSheetService service
    )
    {
        var employeeId = int.Parse(httpRequest.GetClaim(AppConstants.JWT_EMPLOYEE));
        request.EmployeeId = employeeId;
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