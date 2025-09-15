using MachineManagement.API.Controllers;
using MachineManagement.API.Models;
using MachineManagement.API.Result;
using MachineManagement.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MachineManagement.Tests.Controllers;

public class MachineControllerTests
{
    private readonly Mock<IMachineService> _machineService;
    private readonly MachineController _controller;
    
    public MachineControllerTests()
    {
        _machineService = new Mock<IMachineService>();
        _controller = new MachineController(_machineService.Object);
    }
    
    [Fact]
    public async Task GetMachineById_NotFound_Returns_NotFound()
    {
        // Arrange
        _machineService.Setup(s => s.GetMachineByIdAsync(999))
            .ReturnsAsync(Error.NotFound("Machine not found."));

        // Act
        var action = await _controller.GetMachineById(999);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(action.Result);
        Assert.Equal(404, notFound.StatusCode);
    }
    
    [Fact]
    public async Task CreateMachine_BadRequest_Returns_BadRequest()
    {
        // Arrange
        var dto = new CreateMachineDto { Name = "" };
        _machineService.Setup(s => s.CreateMachineAsync(dto))
            .ReturnsAsync(Error.BadRequest("Machine name is required."));

        // Act
        var action = await _controller.CreateMachine(dto);

        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(action.Result);
        Assert.Equal(400, bad.StatusCode);
    }
}