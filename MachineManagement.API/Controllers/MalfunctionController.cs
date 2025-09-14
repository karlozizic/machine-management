using MachineManagement.API.Entities;
using MachineManagement.API.Models;
using MachineManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace MachineManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MalfunctionController :  ControllerBase
{
    private readonly IMalfunctionService _malfunctionService;
    
    public MalfunctionController(IMalfunctionService malfunctionService)
    {
        _malfunctionService = malfunctionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var malfunctions = await _malfunctionService.GetAllAsync();
        return Ok(malfunctions);
    }
    
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<Malfunction>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
            return BadRequest("Page and page size must be greater than zero.");
        
        var pagedResult = await _malfunctionService.GetPagedAsync(page, pageSize);
        return Ok(pagedResult);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var malfunction = await _malfunctionService.GetByIdAsync(id);
        if (malfunction == null)
            return NotFound();
        
        return Ok(malfunction);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMalfunctionDto dto)
    {
        var malfunction = await _malfunctionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = malfunction.Id }, malfunction);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMalfunctionDto dto)
    {
        var malfunction = await _malfunctionService.UpdateAsync(id, dto);
        if (malfunction == null)
            return NotFound();

        return Ok(malfunction);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _malfunctionService.DeleteAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id}/resolve")]
    public async Task<IActionResult> Resolve(int id, [FromQuery] DateTime? end)
    {
        var success = await _malfunctionService.Resolve(id, end);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpGet("machine/{machineId}")]
    public async Task<IActionResult> GetByMachineId(int machineId)
    {
        var malfunctions = await _malfunctionService.GetByMachineIdAsync(machineId);

        return Ok(malfunctions);
    }
}