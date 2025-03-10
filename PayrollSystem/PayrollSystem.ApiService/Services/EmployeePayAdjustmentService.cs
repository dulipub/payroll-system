using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Repositories;
using PayrollSystem.ApiService.Requests.PayAdjustment;

namespace PayrollSystem.ApiService.Services;

public interface IEmployeePayAdjustmentService
{
    public Task<bool> AddPayAdjustmentToEmployees(AddPayAdjustmentToEmployeesRequest request);
    //update amounnts
    //delete
}

public class EmployeePayAdjustmentService(
    IRepository<EmployeePayAdjustment> repository, 
    IEmployeeRepository employeeRepository) : IEmployeePayAdjustmentService
{
    public async Task<bool> AddPayAdjustmentToEmployees(AddPayAdjustmentToEmployeesRequest request)
    {
        if (request.AddToAllEmployees)
        {
            var employees = await employeeRepository.ListAscending(x => x.IsActive == true, x => x.Id);
            var employeeIds = employees.Select(x => x.Id).ToList();
            foreach (var employeeId in employeeIds)
            {
                AddToEmployee(request, employeeId);
            }
        }

        foreach (var employeeId in request.EmployeeIds ?? [])
        {
            AddToEmployee(request, employeeId);
        }
        return true;
    }

    private void AddToEmployee(AddPayAdjustmentToEmployeesRequest request, int employeeId)
    {
        repository.Add(new EmployeePayAdjustment
        {
            EmployeeId = employeeId,
            PayAdjustmentId = request.PayAdjustmentId,
            Amount = request.Amount,
            IsActive = true
        });
    }
}
