using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Models.Identity;

namespace PayrollSystem.ApiService.Core;

public class PayrollDbContext(DbContextOptions<PayrollDbContext> options) : IdentityDbContext<User, Role, string>(options)
{

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<Department> Departments { get; set; }

    public DbSet<TimeSheet> TimeSheets { get; set; }

    public DbSet<Leave> Leaves { get; set; }

    public DbSet<EmployeeProject> EmployeeProjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}