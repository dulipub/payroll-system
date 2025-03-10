using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Models.Identity;

namespace PayrollSystem.ApiService.Core;

public class PayrollDbContext(DbContextOptions<PayrollDbContext> options) : IdentityDbContext<User, Role, string>(options)
{
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeePayAdjustment> EmployeePayAdjustments { get; set; }
    public DbSet<EmployeeProject> EmployeeProjects { get; set; }
    public DbSet<Leave> Leaves { get; set; }
    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<PayAdjustment> PayAdjustments { get; set; }
    public DbSet<Payroll> Payrolls { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Tax> Taxes { get; set; }
    public DbSet<TimeSheet> TimeSheets { get; set; }

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
            new LeaveType { Id = 1, Type = "Annual Leave", MaximumAllowedDays = 14, IsActive = true, IsPaid = true },
            new LeaveType { Id = 2, Type = "Sick Leave", MaximumAllowedDays = 7, IsActive = true, IsPaid = true },
            new LeaveType { Id = 3, Type = "Casual Leave", MaximumAllowedDays = 7, IsActive = true, IsPaid = true },
            new LeaveType { Id = 4, Type = "Nopay Leave", MaximumAllowedDays = 365, IsActive = true, IsPaid = false }
        );

        modelBuilder.Entity<Tax>().HasData(
            new Tax
            {
                Id = 1,
                Description = "PAYEE",
                StartAmount = 0,
                EndAmount = 100000,
                Percentage = 0,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,UpdatedDate = DateTime.UtcNow
            },
            new Tax
            {
                Id = 2,
                Description = "PAYEE",
                StartAmount = 100000,
                EndAmount = 141667,
                Percentage = 0.06,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new Tax
            {
                Id = 2,
                Description = "PAYEE",
                StartAmount = 141667,
                EndAmount = 183333,
                Percentage = 0.12,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new Tax
            {
                Id = 2,
                Description = "PAYEE",
                StartAmount = 183333,
                EndAmount = 225000,
                Percentage = 0.18,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new Tax
            {
                Id = 2,
                Description = "PAYEE",
                StartAmount = 225000,
                EndAmount = 266667,
                Percentage = 0.24,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new Tax
            {
                Id = 2,
                Description = "PAYEE",
                StartAmount = 266667,
                EndAmount = 308333,
                Percentage = 0.30,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new Tax
            {
                Id = 2,
                Description = "PAYEE",
                StartAmount = 308333,
                EndAmount = float.MaxValue - 1,
                Percentage = 0.36,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            }
        );

        modelBuilder.Entity<PayAdjustment>().HasData(
                new PayAdjustment
                {
                    Id = 1,
                    Name = "Incentive",
                    Type = PayAdjustmentType.Allowance,
                    Recurring = true,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                },
                new PayAdjustment
                {
                    Id = 2,
                    Name = "Transport",
                    Type = PayAdjustmentType.Allowance,
                    Recurring = true,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                },
                new PayAdjustment
                {
                    Id = 3,
                    Name = "Medical Claim",
                    Type = PayAdjustmentType.Allowance,
                    Recurring = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                }
        );
    }

}