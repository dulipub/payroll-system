using Mapster;
using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Repositories;
using PayrollSystem.ApiService.Requests.Employee;
using PayrollSystem.ApiService.Requests.Leave;
using PayrollSystem.ApiService.Responses;

namespace PayrollSystem.ApiService.Services;

public interface ILeaveService
{
    Task<LeaveResponse?> Get(int id, int employeeId);
    Task<bool?> Insert(CreateLeaveRequest request);
    Task<bool?> Update(UpdateLeaveRequest request);
    Task<ListResponse<LeaveResponse>> List(LeaveListRequest request);
    Task RemainingLeaves(int employeeId);
}

public class LeaveService(
    IRepository<Leave> repository, 
    IRepository<LeaveType> leaveTypeRepository) : ILeaveService
{
    public async Task<LeaveResponse?> Get(int id, int employeeId)
    {
        var leave = await repository.GetById(id);
        if(leave?.EmployeeId == employeeId)
        {
            return leave.Adapt<LeaveResponse>();
        }

        return null;
    }

    public async Task<bool?> Insert(CreateLeaveRequest request)
    {
        foreach(var leaveRequest in request.LeaveDates)
        {

            if (leaveRequest.LeaveDuration != 0.5f && leaveRequest.LeaveDuration != 1.0f)
            {
                throw new ArgumentException("LeaveDuration must be either 0.5 (half day) or 1.0 (full day).");
            }

            var leave = leaveRequest.Adapt<Leave>();
            leave.IsActive = true;
            leave.IsApproved = false;
            leave.LeaveTypeId = request.LeaveTypeId;
            leave.EmployeeId = request.EmployeeId;

            //check the leaves taken from January 1st
            DateTime januaryFirst = new DateTime(DateTime.Now.Year, 1, 1);
            var leaves = await repository.ListAscending(
                x => x.EmployeeId == request.EmployeeId && x.LeaveTypeId == request.LeaveTypeId && x.Date > januaryFirst,
                x => x.Id,
                "");
            var leaveTypeList = await leaveTypeRepository.ListAscending(x => x.Id == request.LeaveTypeId, x => x.Id);
            var isAvailable = leaveTypeList[0].MaximumAllowedDays - CalculateTotalLeaves(leaves) > 0;

            if (!isAvailable)
                throw new Exception("Leaves are not available, please select a different leave type.");


            await repository.Add(leave);
        }

        return true;
    }

    public async Task<ListResponse<LeaveResponse>> List(LeaveListRequest request)
    {
        var leaves = new List<Leave>();
        var currentYear = DateTime.Now.Year;
        leaves = await repository.ListAscending(
            x => x.EmployeeId == request.EmployeeId && x.Date.Year == currentYear,
            x => x.Id,
            "");

        var results = leaves.Adapt<List<LeaveResponse>>();
        return new ListResponse<LeaveResponse>
        {
            Results = results.Take(request.PageSize).Skip((request.PageNumber - 1) * request.PageSize).ToList(),
            TotalRecords = results.Count
        };
    }

    public async Task<bool?> Update(UpdateLeaveRequest request)
    {
        var leave = await repository.GetById(request.Id);
        if (leave == null) return null;

        request.Adapt(leave);
        var result = await repository.Update(leave);
        return true;
    }

    public async Task RemainingLeaves(int employeeId)
    {
        throw new NotImplementedException();
    }

    private float CalculateTotalLeaves(List<Leave> leaves)
    {
        return leaves.Sum(leave => leave.LeaveDuration);
    }
}