using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Repositories;
using PayrollSystem.ApiService.Requests;
using PayrollSystem.ApiService.Requests.Employee;
using PayrollSystem.ApiService.Responses;

namespace PayrollSystem.ApiService.Services;

public interface IEmployeeService
{
    public Task<EmployeeResponse> GetByUserId(string userId);
    public Task<EmployeeResponse> Insert(InsertEmployeeRequest request);

    //administration
    public Task<EmployeeResponse> Update(string userId, UpdateEmployeeRequest request);
    public Task<ListResponse<EmployeeResponse>> List(ListRequest request);

}


public class EmployeeService(IEmployeeRepository repository) : IEmployeeService
{
    public async Task<EmployeeResponse> GetByUserId(string userId)
    {
        var employee = await repository.GetByUserId(userId);
        var response = employee.Adapt<EmployeeResponse>();

        return response;
    }

    public async Task<EmployeeResponse> Insert(InsertEmployeeRequest request)
    {
        var employee = request.Adapt<Employee>();
        var sucess = await repository.Add(employee);
        if (sucess)
            return employee.Adapt<EmployeeResponse>();
        return null;
    }

    public Task<ListResponse<EmployeeResponse>> List(ListRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<EmployeeResponse> Update(string userId, UpdateEmployeeRequest request)
    {
        throw new NotImplementedException();
    }
}