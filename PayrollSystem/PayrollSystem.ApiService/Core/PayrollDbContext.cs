using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.ApiService.Models;

namespace PayrollSystem.ApiService.Core;

public class PayrollDbContext(DbContextOptions<PayrollDbContext> options) : IdentityDbContext<User, UserRole, string>(options)
{

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}