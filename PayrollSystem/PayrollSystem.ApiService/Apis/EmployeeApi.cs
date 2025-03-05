using Microsoft.Win32;

namespace PayrollSystem.ApiService.Apis;

public static class EmployeeApi
{
    public static RouteGroupBuilder MapEmployeeApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/employee");

        api.MapGet("/profile", Get);
        api.MapPost("/update", Update);

        return api.WithOpenApi();
    }

    private static async Task Get(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task Update(HttpContext context)
    {
        throw new NotImplementedException();
    }
}