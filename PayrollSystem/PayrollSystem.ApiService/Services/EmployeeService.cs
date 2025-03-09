using Mapster;
using Microsoft.AspNetCore.Identity;
using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Models.Identity;
using PayrollSystem.ApiService.Repositories;
using PayrollSystem.ApiService.Requests.Employee;
using PayrollSystem.ApiService.Responses;

namespace PayrollSystem.ApiService.Services;

public interface IEmployeeService
{
    public Task<EmployeeResponse> GetByUserId(string userId);
    public Task<EmployeeResponse> Insert(InsertEmployeeRequest request);
    public Task<EmployeeResponse> Update(string userId, UpdateEmployeeRequest request);
    public Task<ListResponse<EmployeeResponse>> List(EmployeeListRequest request);

}


public class EmployeeService(
    IEmployeeRepository repository, 
    UserManager<User> userManager,
    IRepository<BankAccount> bankAccountRepository,
    IRepository<Project> projectRepository,
    IRepository<EmployeeProject> employeeProjectRepository) : IEmployeeService
{
    public async Task<EmployeeResponse> GetByUserId(string userId)
    {
        var employee = await repository.GetByUserId(userId);
        var response = employee.Adapt<EmployeeResponse>();

        return response;
    }

    public async Task<EmployeeResponse> Insert(InsertEmployeeRequest request)
    {
        string userId = request.UserId ?? string.Empty;
        if (request.UserId == null)
        {
            var newUser = await CreateUser(request);
            userId = newUser.Id;
        }

        var employee = request.Adapt<Employee>();
        employee.UserId = userId;
        var sucess = await repository.Add(employee);
        
        if (sucess)
        {
            if (request.BankAccount != null)
            {
                var bank = request.BankAccount.Adapt<BankAccount>();
                bank.EmployeeId = employee.Id;
                await bankAccountRepository.Add(bank);
            }

            return employee.Adapt<EmployeeResponse>();
        }
        return null;
    }

    public async Task<ListResponse<EmployeeResponse>> List(EmployeeListRequest request)
    {
        var employees = new List<Employee>();
        if (request.ProjectId > 0)
        {
            employees = await SearchByProject(request);
        }
        else if (request.DepartmentId > 0)
        {
            employees = await SearchByDepartment(request);
        }
        else
        {
            employees = await repository.ListAscending(
                e => string.IsNullOrEmpty(request.SearchTerm) || e.Name.Contains(request.SearchTerm),
                e => e.Id,
                "");
        }

        var results = employees.Adapt<List<EmployeeResponse>>();
        return new ListResponse<EmployeeResponse> { 
            Results = results.Take(request.PageSize).Skip((request.PageNumber -1) * request.PageSize).ToList(),
            TotalRecords = results.Count
        };
    }

    public async Task<EmployeeResponse> Update(string userId, UpdateEmployeeRequest request)
    {
        throw new NotImplementedException();
    }

    private async Task<User> CreateUser(InsertEmployeeRequest request)
    {
        var existingUser = await userManager.FindByEmailAsync(request.User.Email);
        if (existingUser != null)
            throw new Exception($"User with this email already exists, UserId : {existingUser.Id}");
        existingUser = await userManager.FindByNameAsync(request.User.Username);
        if (existingUser != null)
            throw new Exception($"User with this username already exists, UserId : {existingUser.Id}");

        var user = new User
        {
            UserName = request.User.Username,
            Email = request.User.Email,
            TwoFactorEnabled = false
        };

        await userManager.CreateAsync(user, request.User.Password);
        return user;
    }

    private async Task<List<Employee>> SearchByDepartment(EmployeeListRequest request)
    {
        var projects = await projectRepository.ListAscending(p => p.DepartmentId == request.DepartmentId);
        var projectIds = projects.Select(p => p.Id);
        var employeesInProject = await employeeProjectRepository.ListAscending(ep => projectIds.Contains(ep.Id), ep => ep.Id);
        var employeeIds = employeesInProject.Select(e => e.Id).ToList();

        var employees = await repository.ListAscending(
            e => employeeIds.Contains(e.Id) && (string.IsNullOrEmpty(request.SearchTerm) || e.Name.Contains(request.SearchTerm)),
            e => e.Id,
            "");
        return employees;
    }

    private async Task<List<Employee>> SearchByProject(EmployeeListRequest request)
    {
        var employeesInProject = await employeeProjectRepository.ListAscending(ep => ep.ProjectId == request.ProjectId, ep => ep.Id);
        var employeeIds = employeesInProject.Select(e => e.Id).ToList();

        var employees = await repository.ListAscending(
            e => employeeIds.Contains(e.Id) && (string.IsNullOrEmpty(request.SearchTerm) || e.Name.Contains(request.SearchTerm)),
            e => e.Id,
            "");
        return employees;
    }

}