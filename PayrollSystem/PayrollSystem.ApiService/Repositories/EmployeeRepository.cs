using Microsoft.EntityFrameworkCore;
using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Responses;

namespace PayrollSystem.ApiService.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    public Task<Employee> GetByUserId(string userId);
}

public class EmployeeRepository(PayrollDbContext context) : BaseRepository<Employee>(context), IEmployeeRepository
{
    public async Task<Employee> GetByUserId(string userId)
    {
        var employee = await context.Employees.FirstOrDefaultAsync(e => e.UserId == userId); 
        return employee;
    }
}