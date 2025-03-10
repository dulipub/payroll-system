using Moq;
using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Repositories;
using PayrollSystem.ApiService.Requests.Employee;
using PayrollSystem.ApiService.Responses;
using PayrollSystem.ApiService.Services;
using Xunit;

namespace PayrollSystem.UnitTests;
public class EmployeeServiceTests
{
    private readonly Mock<IRepository<Employee>> _employeeRepositoryMock;
    private readonly IEmployeeService _employeeService;

    public EmployeeServiceTests()
    {
        _employeeRepositoryMock = new Mock<IRepository<Employee>>();
        _employeeService = new EmployeeService(_employeeRepositoryMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnEmployee_WhenEmployeeExists()
    {
        // Arrange
        var employeeId = 1;
        var employee = new Employee { Id = employeeId, Name = "John Doe" };
        _employeeRepositoryMock.Setup(repo => repo.GetById(employeeId)).ReturnsAsync(employee);

        // Act
        var result = await _employeeService.Get(employeeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(employeeId, result.Id);
    }

    [Fact]
    public async Task Insert_ShouldReturnTrue_WhenEmployeeIsInserted()
    {
        // Arrange
        var request = new CreateEmployeeRequest { Name = "John Doe", Address = "123 Main St", NIC = "123456789" };
        _employeeRepositoryMock.Setup(repo => repo.Add(It.IsAny<Employee>())).ReturnsAsync(true);

        // Act
        var result = await _employeeService.Insert(request);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Update_ShouldReturnTrue_WhenEmployeeIsUpdated()
    {
        // Arrange
        var request = new UpdateEmployeeRequest { Id = 1, Name = "John Doe", Address = "123 Main St", NIC = "123456789" };
        _employeeRepositoryMock.Setup(repo => repo.Update(It.IsAny<Employee>())).ReturnsAsync(true);

        // Act
        var result = await _employeeService.Update(request);

        // Assert
        Assert.True(result);
    }
}
