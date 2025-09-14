using MachineManagement.API.Entities;
using MachineManagement.API.Models;
using MachineManagement.API.Result;
using MachineManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace MachineManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MachineController : ControllerBase
{
    private readonly IMachineService _machineService;

    public MachineController(IMachineService machineService)
    {
        _machineService = machineService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Machine>>> GetMachines()
    {
        var machinesResult = await _machineService.GetAllMachinesAsync();

        return machinesResult.ToActionResult();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Machine?>> GetMachineById(int id)
    {
        var machineResult = await _machineService.GetMachineByIdAsync(id);
        return machineResult.ToActionResult();    
    }

    [HttpPost]
    public async Task<ActionResult<Machine>> CreateMachine([FromBody] CreateMachineDto dto)
    {
        var machineResult = await _machineService.CreateMachineAsync(dto);
        
        return machineResult.ToActionResult();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Machine?>> UpdateMachine(int id, [FromBody] UpdateMachineDto dto)
    {
        var machineResult = await _machineService.UpdateMachineAsync(id, dto);
        return machineResult.ToActionResult();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteMachine(int id)
    {
        var result = await _machineService.DeleteMachineAsync(id);
        return result.ToActionResult();
    }
}