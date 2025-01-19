using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.ApiService.Core;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.AddNpgsqlDbContext<PayrollDbContext>(connectionName: "postgresdb");


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddAuthServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseAuthentication();
//app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.MapDefaultEndpoints();

app.Run();