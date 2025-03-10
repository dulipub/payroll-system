using Moq;
using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Repositories;
using PayrollSystem.ApiService.Requests.Leave;
using PayrollSystem.ApiService.Responses;
using PayrollSystem.ApiService.Services;
using Xunit;

namespace PayrollSystem.UnitTests;
public class LeaveServiceTests
{
    private readonly Mock<IRepository<Leave>> _leaveRepositoryMock;
    private readonly Mock<IRepository<LeaveType>> _leaveTypeRepositoryMock;
    private readonly ILeaveService _leaveService;

    public LeaveServiceTests()
    {
        _leaveRepositoryMock = new Mock<IRepository<Leave>>();
        _leaveTypeRepositoryMock = new Mock<IRepository<LeaveType>>();
        _leaveService = new LeaveService(_leaveRepositoryMock.Object, _leaveTypeRepositoryMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnLeave_WhenLeaveExists()
    {
        // Arrange
        var leaveId = 1;
        var employeeId = 1;
        var leave = new Leave { Id = leaveId, Date = DateOnly.FromDateTime(DateTime.Now), EmployeeId = employeeId };
        _leaveRepositoryMock.Setup(repo => repo.GetById(leaveId)).ReturnsAsync(leave);

        // Act
        var result = await _leaveService.Get(leaveId, employeeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(leaveId, result.Id);
    }

    [Fact]
    public async Task Insert_ShouldReturnTrue_WhenLeaveIsInserted()
    {
        // Arrange
        var request = new CreateLeaveRequest
        {
            EmployeeId = 1,
            LeaveTypeId = 1,
            LeaveDates = new List<LeaveDateDto>
            {
                new LeaveDateDto { Date = DateOnly.FromDateTime(DateTime.Now), LeaveDuration = 1.0 }
            }
        };
        var leaveType = new LeaveType { Id = request.LeaveTypeId, Type = "Casual Leave",  MaximumAllowedDays = 10 };
        _leaveTypeRepositoryMock.Setup(repo => repo.GetById(request.LeaveTypeId)).ReturnsAsync(leaveType);
        _leaveRepositoryMock.Setup(repo => repo.Add(It.IsAny<Leave>())).ReturnsAsync(true);

        // Act
        var result = await _leaveService.Insert(request);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Update_ShouldReturnTrue_WhenLeaveIsUpdated()
    {
        // Arrange
        var request = new UpdateLeaveRequest { Id = 1, Date = DateOnly.FromDateTime(DateTime.Now), EmployeeId = 1, LeaveTypeId = 1, LeaveDuration = 1.0 };
        var leave = new Leave { Id = request.Id, Date = DateOnly.FromDateTime(DateTime.Now), EmployeeId = request.EmployeeId };
        _leaveRepositoryMock.Setup(repo => repo.GetById(request.Id)).ReturnsAsync(leave);
        _leaveRepositoryMock.Setup(repo => repo.Update(It.IsAny<Leave>())).ReturnsAsync(true);

        // Act
        var result = await _leaveService.Update(request);

        // Assert
        Assert.True(result);
    }
}
