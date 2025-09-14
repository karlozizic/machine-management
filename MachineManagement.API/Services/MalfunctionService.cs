using MachineManagement.API.Models;
using MachineManagement.API.Repositories;

namespace MachineManagement.API.Services;

public class MalfunctionService : IMalfunctionService
{
    private readonly IMalfunctionRepository _malfunctionRepository;
    
    public MalfunctionService(IMalfunctionRepository malfunctionRepository)
    {
        _malfunctionRepository = malfunctionRepository;
    }

    public async Task<IEnumerable<Malfunction>> GetAllAsync()
    {
        return await _malfunctionRepository.GetAllAsync();
    }
    
    public async Task<PagedResult<Malfunction>> GetPagedAsync(int page, int pageSize)
    {
        return await _malfunctionRepository.GetPagedAsync(page, pageSize);
    }

    public async Task<Malfunction?> GetByIdAsync(int id)
    {
        return await _malfunctionRepository.GetByIdAsync(id);
    }

    public async Task<Malfunction> CreateAsync(CreateMalfunctionDto dto)
    {
        return await _malfunctionRepository.CreateAsync(dto);
    }

    public async Task<Malfunction?> UpdateAsync(int id, UpdateMalfunctionDto dto)
    {
        return await _malfunctionRepository.UpdateAsync(id, dto);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _malfunctionRepository.DeleteAsync(id);
    }

    public async Task<bool> Resolve(int id, DateTime? time = null)
    {
        return await _malfunctionRepository.Resolve(id, time);
    }
    
    public async Task<IEnumerable<Malfunction>> GetByMachineIdAsync(int machineId)
    {
        return await _malfunctionRepository.GetByMachineIdAsync(machineId);
    }
}

public interface IMalfunctionService
{
    Task<IEnumerable<Malfunction>> GetAllAsync();
    Task<PagedResult<Malfunction>> GetPagedAsync(int page, int pageSize);
    Task<Malfunction?> GetByIdAsync(int id);
    Task<Malfunction> CreateAsync(CreateMalfunctionDto dto);
    Task<Malfunction?> UpdateAsync(int id, UpdateMalfunctionDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> Resolve(int id, DateTime? time = null);
    Task<IEnumerable<Malfunction>> GetByMachineIdAsync(int machineId);
}