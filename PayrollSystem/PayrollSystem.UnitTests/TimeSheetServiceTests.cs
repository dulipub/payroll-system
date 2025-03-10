using Moq;
using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Repositories;
using PayrollSystem.ApiService.Requests.TimeSheet;
using PayrollSystem.ApiService.Responses;
using PayrollSystem.ApiService.Services;
using Xunit;

namespace PayrollSystem.UnitTests;
public class TimeSheetServiceTests
{
    private readonly Mock<IRepository<TimeSheet>> _timeSheetRepositoryMock;
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly ITimeSheetService _timeSheetService;

    public TimeSheetServiceTests()
    {
        _timeSheetRepositoryMock = new Mock<IRepository<TimeSheet>>();
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _timeSheetService = new TimeSheetService(_timeSheetRepositoryMock.Object, _employeeRepositoryMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnTimeSheet_WhenTimeSheetExists()
    {
        // Arrange
        var timeSheetId = 1;
        var employeeId = 1;
        var timeSheet = new TimeSheet { Id = timeSheetId, EmployeeId = employeeId };
        _timeSheetRepositoryMock.Setup(repo => repo.GetById(timeSheetId)).ReturnsAsync(timeSheet);

        // Act
        var result = await _timeSheetService.Get(timeSheetId, employeeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(timeSheetId, result.Id);
    }

    [Fact]
    public async Task Insert_ShouldReturnTrue_WhenTimeSheetIsInserted()
    {
        // Arrange
        var request = new CreateTimeSheetRequest { EmployeeId = 1, Date = DateOnly.FromDateTime(DateTime.Now), HoursWorked = 8 };
        var employee = new Employee { Id = request.EmployeeId };
        _employeeRepositoryMock.Setup(repo => repo.GetById(request.EmployeeId)).ReturnsAsync(employee);
        _timeSheetRepositoryMock.Setup(repo => repo.Add(It.IsAny<TimeSheet>())).ReturnsAsync(true);

        // Act
        var result = await _timeSheetService.Insert(request);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Update_ShouldReturnTrue_WhenTimeSheetIsUpdated()
    {
        // Arrange
        var request = new UpdateTimeSheetRequest { Id = 1, EmployeeId = 1, Date = DateOnly.FromDateTime(DateTime.Now), HoursWorked = 8 };
        var employee = new Employee { Id = request.EmployeeId };
        _employeeRepositoryMock.Setup(repo => repo.GetById(request.EmployeeId)).ReturnsAsync(employee);
        _timeSheetRepositoryMock.Setup(repo => repo.Update(It.IsAny<TimeSheet>())).ReturnsAsync(true);

        // Act
        var result = await _timeSheetService.Update(request);

        // Assert
        Assert.True(result);
    }
}
