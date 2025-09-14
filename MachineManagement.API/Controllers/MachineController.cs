using MachineManagement.API.Models;
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
    public async Task<IActionResult> GetMachines()
    {
        var machines = await _machineService.GetAllMachinesAsync();

        return Ok(machines);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMachineById(int id)
    {
        var machine = await _machineService.GetMachineByIdAsync(id);

        if (machine == null)
            return NotFound();

        return Ok(machine);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMachine([FromBody] CreateMachineDto dto)
    {
        var machine = await _machineService.CreateMachineAsync(dto);

        return CreatedAtAction(nameof(GetMachineById), new { id = machine.Id }, machine);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMachine(int id, [FromBody] UpdateMachineDto dto)
    {
        var machine = await _machineService.UpdateMachineAsync(id, dto);

        if (machine == null)
            return NotFound();

        return Ok(machine);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMachine(int id)
    {
        var success = await _machineService.DeleteMachineAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}