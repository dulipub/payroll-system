using Mapster;
using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Repositories;
using PayrollSystem.ApiService.Requests.TimeSheet;
using PayrollSystem.ApiService.Responses;

namespace PayrollSystem.ApiService.Services;

public interface ITimeSheetService
{
    Task<TimeSheetResponse?> Get(int id, int employeeId);
    Task<bool> Insert(CreateTimeSheetRequest request);
    Task<bool> Update(UpdateTimeSheetRequest request);
    Task<ListResponse<TimeSheetResponse>> List(TimeSheetListRequest request);
}

public class TimeSheetService(IRepository<TimeSheet> timeSheetRepository, IEmployeeRepository employeeRepository) : ITimeSheetService
{
    public async Task<TimeSheetResponse?> Get(int id, int employeeId)
    {
        var timeSheet = await timeSheetRepository.GetById(id);
        if (timeSheet?.EmployeeId == employeeId)
        {
            return timeSheet.Adapt<TimeSheetResponse>();
        }
        return null;
    }
    public async Task<bool> Insert(CreateTimeSheetRequest request)
    {
        var employee = await employeeRepository.GetById(request.EmployeeId);
        if (employee == null)
        {
            return false;
        }

        var timeSheet = request.Adapt<TimeSheet>();
        return await timeSheetRepository.Add(timeSheet);
    }
    public async Task<bool> Update(UpdateTimeSheetRequest request)
    {
        var employee = await employeeRepository.GetById(request.EmployeeId);
        if (employee == null)
        {
            return false;
        }
        var timeSheet = request.Adapt<TimeSheet>();
        return await timeSheetRepository.Update(timeSheet);
    }
    public async Task<ListResponse<TimeSheetResponse>> List(TimeSheetListRequest request)
    {
        var timeSheets = new List<TimeSheet>();
        if (request.EmployeeId > 0)
        {
            timeSheets = await timeSheetRepository.ListAscending(x => x.EmployeeId == request.EmployeeId, x => x.Id);
            var results = timeSheets.Adapt<List<TimeSheetResponse>>();
            return new ListResponse<TimeSheetResponse>
            {
                Results = results.Take(request.PageSize).Skip((request.PageNumber - 1) * request.PageSize).ToList(),
                TotalRecords = results.Count
            };
        }

        return new ListResponse<TimeSheetResponse> { 
            Results = timeSheets.Adapt<List<TimeSheetResponse>>(),
            TotalRecords = timeSheets.Count
        };
    }
}