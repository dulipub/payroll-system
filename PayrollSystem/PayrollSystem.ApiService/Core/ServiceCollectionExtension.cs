using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Models.Identity;
using PayrollSystem.ApiService.Repositories;
using PayrollSystem.ApiService.Services;
using System.Text;

namespace PayrollSystem.ApiService.Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        });

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<PayrollDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("Manager", policy => policy.RequireRole("Manager"));
            options.AddPolicy("HumanResources", policy => policy.RequireRole("HumanResources"));
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        services.AddTransient<IRepository<Project>, BaseRepository<Project>>();
        services.AddTransient<IRepository<Department>, BaseRepository<Department>>();
        services.AddTransient<IRepository<EmployeeProject>, BaseRepository<EmployeeProject>>();
        services.AddTransient<IRepository<Leave>, BaseRepository<Leave>>();
        services.AddTransient<IRepository<TimeSheet>, BaseRepository<TimeSheet>>();
        services.AddTransient<IRepository<BankAccount>, BaseRepository<BankAccount>>();

        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IEmployeeService, EmployeeService>();

        return services;
    }

    public static async Task AddIdentityRoles(this IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            string[] roleNames = { "Employee", "Manager", "HumanResources", "Admin" };

            foreach (var role in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Role { Name = role });
                }
            }
        }

    }
}