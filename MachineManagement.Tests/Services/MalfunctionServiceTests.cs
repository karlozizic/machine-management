using MachineManagement.API.Entities;
using MachineManagement.API.Models;
using MachineManagement.API.Repositories;
using MachineManagement.API.Services;
using Moq;

namespace MachineManagement.Tests.Services;

public class MalfunctionServiceTests
{
    private readonly Mock<IMalfunctionRepository> _malfunctionRepositoryMock;
    private readonly Mock<IMachineRepository> _machineRepositoryMock;
    private readonly MalfunctionService _malfunctionService;
    
    public MalfunctionServiceTests()
    {
        _malfunctionRepositoryMock = new Mock<IMalfunctionRepository>();
        _machineRepositoryMock = new Mock<IMachineRepository>();
        _malfunctionService = new MalfunctionService(_malfunctionRepositoryMock.Object, _machineRepositoryMock.Object);
    }
    
    [Fact]
    public async Task BadRequest_When_Description_Missing()
    {
        // Arrange
        var dto = new CreateMalfunctionDto
        {
            Name = "Test Malfunction",
            Description = "",
            MachineId = 1,
            Priority = Priority.Medium
        };
        
        // Act
        var result = await _malfunctionService.CreateAsync(dto);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Description is required.", result.Error.Description);
    }
    
    [Fact]
    public async Task BadRequest_When_Machine_Does_Not_Exist()
    {
        // Arrange
        var dto = new CreateMalfunctionDto
        {
            Name = "Test Malfunction",
            Description = "desc",
            MachineId = 1,
            Priority = Priority.Medium
        };

        _machineRepositoryMock
            .Setup(r => r.ExistsByIdAsync(1))
            .ReturnsAsync(false);

        // Act
        var result = await _malfunctionService.CreateAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Machine with the given ID does not exist.", result.Error.Description);
    }
}