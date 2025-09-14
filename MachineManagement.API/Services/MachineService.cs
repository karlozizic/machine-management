using MachineManagement.API.Models;
using MachineManagement.API.Repositories;

namespace MachineManagement.API.Services;

public class MachineService : IMachineService
{
    private readonly IMachineRepository _machineRepository;
    
    public MachineService(IMachineRepository machineRepository)
    {
        _machineRepository = machineRepository;
    }
    
    public async Task<IEnumerable<Machine>> GetAllMachinesAsync()
    {
        return await _machineRepository.GetAllAsync();
    }

    public async Task<Machine?> GetMachineByIdAsync(int id)
    {
        return await _machineRepository.GetByIdAsync(id);
    }

    public async Task<Machine> CreateMachineAsync(CreateMachineDto dto)
    {
        return await _machineRepository.CreateAsync(dto);
    }

    public async Task<Machine?> UpdateMachineAsync(int id, UpdateMachineDto dto)
    {
        return await _machineRepository.UpdateAsync(id, dto);
    }

    public async Task<bool> DeleteMachineAsync(int id)
    {
        return await _machineRepository.DeleteAsync(id);
    }
}

public interface IMachineService
{
    Task<IEnumerable<Machine>> GetAllMachinesAsync();
    Task<Machine?> GetMachineByIdAsync(int id);
    Task<Machine> CreateMachineAsync(CreateMachineDto dto);
    Task<Machine?> UpdateMachineAsync(int id, UpdateMachineDto dto);
    Task<bool> DeleteMachineAsync(int id);
}