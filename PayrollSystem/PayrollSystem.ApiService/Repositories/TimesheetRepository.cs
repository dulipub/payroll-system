using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Models;

namespace PayrollSystem.ApiService.Repositories;

public interface ITimesheetRepository : IRepository<TimeSheet>
{
    Task<List<TimeSheet>> GetByMonth(int Year, int Month, int EmployeeId);
}

public class TimesheetRepository(PayrollDbContext context) : BaseRepository<TimeSheet>(context), ITimesheetRepository
{
    public async Task<List<TimeSheet>> GetByMonth(int Year, int Month, int EmployeeId)
    {
        //get 1st date using year and month
        var startDate = new DateOnly(Year, Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var timesheets = await ListAscending(x => x.Date >= startDate && x.Date <= endDate && x.EmployeeId == EmployeeId);
        return timesheets;
    }
}