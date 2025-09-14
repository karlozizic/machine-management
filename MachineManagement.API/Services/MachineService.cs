using MachineManagement.API.Entities;
using MachineManagement.API.Models;
using MachineManagement.API.Repositories;
using MachineManagement.API.Result;

namespace MachineManagement.API.Services;

public class MachineService : IMachineService
{
    private readonly IMachineRepository _machineRepository;
    
    public MachineService(IMachineRepository machineRepository)
    {
        _machineRepository = machineRepository;
    }
    
    public async Task<Result<IEnumerable<Machine>>> GetAllMachinesAsync()
    {
        var machines =  await _machineRepository.GetAllAsync();
        return Result<IEnumerable<Machine>>.Success(machines);
    }

    public async Task<Result<Machine>> GetMachineByIdAsync(int id)
    {
        if (id <= 0)
            return Error.BadRequest("Invalid machine ID.");
        
        var machine = await _machineRepository.GetByIdAsync(id);
        
        if (machine == null)
            return Error.NotFound("Machine not found.");

        return machine;
    }

    public async Task<Result<Machine>> CreateMachineAsync(CreateMachineDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return Error.BadRequest("Machine name is required.");
        
        if (await _machineRepository.ExistsAsync(dto.Name))
            return Error.BadRequest("Machine with the same name already exists.");
        
        return await _machineRepository.CreateAsync(dto);
    }

    public async Task<Result<Machine>> UpdateMachineAsync(int id, UpdateMachineDto dto)
    {
        if (id <= 0)
            return Error.BadRequest("Invalid machine ID.");
         
        if (string.IsNullOrWhiteSpace(dto.Name))
            return Error.BadRequest("Machine name is required.");
        
        if (await _machineRepository.ExistsAsync(dto.Name))
            return Error.BadRequest("Another machine with the same name already exists.");
        
        if (!await _machineRepository.ExistsByIdAsync(id))
            return Error.NotFound("Machine not found.");
        
        var updatedMachine = await _machineRepository.UpdateAsync(id, dto);
        
        if (updatedMachine == null)
            return Error.InternalServerError("Failed to update machine.");
        
        return updatedMachine;
    }

    public async Task<Result<bool>> DeleteMachineAsync(int id)
    {
        if (id <= 0)
            return Error.BadRequest("Invalid machine ID.");
        
        if (!await _machineRepository.ExistsByIdAsync(id))
            return Error.NotFound("Machine not found.");
        
        var deleted = await _machineRepository.DeleteAsync(id);
        
        if (!deleted)
            return Error.InternalServerError("Failed to delete machine.");

        return true;
    }
}

public interface IMachineService
{
    Task<Result<IEnumerable<Machine>>> GetAllMachinesAsync();
    Task<Result<Machine>> GetMachineByIdAsync(int id);
    Task<Result<Machine>> CreateMachineAsync(CreateMachineDto dto);
    Task<Result<Machine>> UpdateMachineAsync(int id, UpdateMachineDto dto);
    Task<Result<bool>> DeleteMachineAsync(int id);
}