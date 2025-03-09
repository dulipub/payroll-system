
using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Apis;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//add appsettings files
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.local.json")
    .AddEnvironmentVariables();

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.AddNpgsqlDbContext<PayrollDbContext>(connectionName: "postgresdb");

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//authenticatio
builder.Services.AddAuthServices(builder.Configuration);

//services & repositories
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddAppServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

//swagger
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Payroll API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

//API endpoints
app.MapUserApi();
app.MapEmployeeApi();

await app.Services.AddIdentityRoles();
app.Run();