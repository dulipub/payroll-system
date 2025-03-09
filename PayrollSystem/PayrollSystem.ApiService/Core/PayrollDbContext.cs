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
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<LeaveType> LeaveTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Leave>()
        .Property(e => e.Date)
        .HasConversion(
            v => v.ToUniversalTime(),  // Convert to UTC when saving
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Ensure UTC when reading
        );

        modelBuilder.Entity<LeaveType>().HasData(
            new LeaveType { Id = 1, Type = "Annual Leave", MaximumAllowedDays = 14, IsActive = true },
            new LeaveType { Id = 2, Type = "Sick Leave", MaximumAllowedDays = 7, IsActive = true },
            new LeaveType { Id = 3, Type = "Casual Leave", MaximumAllowedDays = 7, IsActive = true }
        );
    }

}