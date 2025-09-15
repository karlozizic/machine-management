using MachineManagement.API.Entities;
using MachineManagement.API.Models;
using MachineManagement.API.Repositories;
using MachineManagement.API.Result;

namespace MachineManagement.API.Services;

public class MalfunctionService : IMalfunctionService
{
    private readonly IMalfunctionRepository _malfunctionRepository;
    private readonly IMachineRepository _machineRepository;
    
    public MalfunctionService(IMalfunctionRepository malfunctionRepository,
        IMachineRepository machineRepository)
    {
        _malfunctionRepository = malfunctionRepository;
        _machineRepository = machineRepository;
    }

    public async Task<Result<IEnumerable<Malfunction>>> GetAllAsync()
    {
        var malfunctions = await _malfunctionRepository.GetAllAsync();
        return Result<IEnumerable<Malfunction>>.Success(malfunctions);
    }
    
    //TODO: add filtering, sorting
    public async Task<Result<PagedResult<Malfunction>>> GetPagedAsync(int page, int pageSize)
    {
        return await _malfunctionRepository.GetPagedAsync(page, pageSize);
    }

    public async Task<Result<Malfunction>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Error.BadRequest("Invalid malfunction ID.");
        
        var malfunction = await _malfunctionRepository.GetByIdAsync(id);
        
        if (malfunction == null)
            return Error.NotFound("Malfunction not found.");
        
        return malfunction;
    }

    public async Task<Result<Malfunction>> CreateAsync(CreateMalfunctionDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Description))
            return Error.BadRequest("Description is required.");
        
        if (dto.MachineId <= 0)
            return Error.BadRequest("Invalid MachineId.");
        
        if (!await _machineRepository.ExistsByIdAsync(dto.MachineId))
            return Error.BadRequest("Machine with the given ID does not exist.");
        
        if (await _malfunctionRepository.HasActiveMalfunctionsAsync(dto.MachineId))
            return Error.Conflict("There is already an active malfunction for the specified machine.");
        
        return await _malfunctionRepository.CreateAsync(dto);
    }

    public async Task<Result<Malfunction>> UpdateAsync(int id, UpdateMalfunctionDto dto)
    {
        if (id <= 0)
            return Error.BadRequest("Invalid malfunction ID.");
        
        if (string.IsNullOrWhiteSpace(dto.Description))        
            return Error.BadRequest("Description is required.");
        
        if (!await _malfunctionRepository.ExistsByIdAsync(id))
            return Error.NotFound("Malfunction not found.");
            
        var updatedMalfunction = await _malfunctionRepository.UpdateAsync(id, dto);
        
        if (updatedMalfunction == null)
            return Error.InternalServerError("Failed to update malfunction.");
        
        return updatedMalfunction;
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return Error.BadRequest("Invalid malfunction ID.");
        
        if (!await _malfunctionRepository.ExistsByIdAsync(id))
            return Error.NotFound("Malfunction not found.");
        
        var deleted = await _malfunctionRepository.DeleteAsync(id);
        
        if (!deleted)
            return Error.InternalServerError("Failed to delete malfunction.");
        
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> Resolve(int id, DateTime? time = null)
    {
        if (id <= 0)
            return Error.BadRequest("Invalid malfunction ID.");
        
        if (!await _malfunctionRepository.ExistsByIdAsync(id))
            return Error.NotFound("Malfunction not found.");
        
        var resolve = await _malfunctionRepository.Resolve(id, time);
        
        if (!resolve)
            return Error.InternalServerError("Failed to resolve malfunction.");
        
        return Result<bool>.Success(true);
    }
    
    public async Task<Result<IEnumerable<Malfunction>>> GetByMachineIdAsync(int machineId)
    {
        var malfunctions = await _malfunctionRepository.GetByMachineIdAsync(machineId);
        return Result<IEnumerable<Malfunction>>.Success(malfunctions);
    }
}

public interface IMalfunctionService
{
    Task<Result<IEnumerable<Malfunction>>> GetAllAsync();
    Task<Result<PagedResult<Malfunction>>> GetPagedAsync(int page, int pageSize);
    Task<Result<Malfunction>> GetByIdAsync(int id);
    Task<Result<Malfunction>> CreateAsync(CreateMalfunctionDto dto);
    Task<Result<Malfunction>> UpdateAsync(int id, UpdateMalfunctionDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<bool>> Resolve(int id, DateTime? time = null);
    Task<Result<IEnumerable<Malfunction>>> GetByMachineIdAsync(int machineId);
}