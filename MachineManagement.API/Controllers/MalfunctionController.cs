using MachineManagement.API.Entities;
using MachineManagement.API.Models;
using MachineManagement.API.Result;
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
    public async Task<ActionResult<IEnumerable<Malfunction>>> GetAll()
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
    public async Task<ActionResult<Malfunction>> GetById(int id)
    {
        var malfunctionResult = await _malfunctionService.GetByIdAsync(id);
        return malfunctionResult.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<Malfunction>> Create([FromBody] CreateMalfunctionDto dto)
    {
        var malfunctionResult = await _malfunctionService.CreateAsync(dto);
        return malfunctionResult.ToActionResult();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Malfunction>> Update(int id, [FromBody] UpdateMalfunctionDto dto)
    {
        var malfunctionResult = await _malfunctionService.UpdateAsync(id, dto);
        
        return Ok(malfunctionResult);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var success = await _malfunctionService.DeleteAsync(id);
        return success.ToActionResult();
    }

    [HttpPost("{id}/resolve")]
    public async Task<ActionResult<bool>> Resolve(int id, [FromQuery] DateTime? end)
    {
        var success = await _malfunctionService.Resolve(id, end);
        return success.ToActionResult();
    }

    [HttpGet("machine/{machineId}")]
    public async Task<ActionResult<IEnumerable<Malfunction>>> GetByMachineId(int machineId)
    {
        var malfunctions = await _malfunctionService.GetByMachineIdAsync(machineId);
        return malfunctions.ToActionResult();
    }
}