using System.Data;
using Dapper;
using MachineManagement.API.Entities;
using MachineManagement.API.Models;

namespace MachineManagement.API.Repositories;

public class MachineRepository : IMachineRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly IMalfunctionRepository _malfunctionRepository;
    
    public MachineRepository(IDbConnection dbConnection,
        IMalfunctionRepository malfunctionRepository)
    {
        _dbConnection = dbConnection;
        _malfunctionRepository = malfunctionRepository;
    }
    
    public async Task<IEnumerable<Machine>> GetAllAsync()
    {
        //TODO: repair time - denormalized field instead of calculated value?
        //EPOCH - seconds
        const string query = @"
            SELECT 
                m.id,
                m.name,
                m.created_at as CreatedAt,
                m.updated_at as UpdatedAt,
                COALESCE(AVG(EXTRACT(EPOCH FROM (mf.end_time - mf.start_time)) / 3600.0), 0) as AverageRepairTime
            FROM machines m
            LEFT JOIN malfunctions mf ON m.Id = mf.machine_id
                AND mf.end_time IS NOT NULL
                AND mf.is_resolved = true
            GROUP BY m.id, m.name, m.created_at, m.updated_at
            ORDER BY m.name";

        var machines = await _dbConnection.QueryAsync<Machine>(query);
        return machines;
    }

    public async Task<Machine?> GetByIdAsync(int id)
    {
        //EPOCH - seconds
        const string query = @"
            SELECT 
                m.id,
                m.name,
                m.created_at as CreatedAt,
                m.updated_at as UpdatedAt,
                COALESCE(AVG(EXTRACT(EPOCH FROM (mf.end_time - mf.start_time)) / 3600.0), 0) as AverageRepairTime
            FROM machines m
            LEFT JOIN malfunctions mf ON m.id = mf.machine_id
                AND mf.end_time IS NOT NULL
                AND mf.is_resolved = true
            WHERE m.id = @Id
            GROUP BY m.id, m.name, m.created_at, m.updated_at";
        
        var machine = await _dbConnection.QueryFirstOrDefaultAsync<Machine>(query, new { Id = id });

        if (machine == null)
            return null;
        
        // Fetch malfunctions for the machine
        var malfunctions = await _malfunctionRepository.GetByMachineIdAsync(id);
        machine.Malfunctions = malfunctions.ToList();
        
        return machine;
    }

    public async Task<Machine> CreateAsync(CreateMachineDto dto)
    {
        const string query = @"
            INSERT INTO machines (name)
            VALUES (@name)
            RETURNING id, name, created_at as CreatedAt, updated_at as UpdatedAt";
    
        var machine = await _dbConnection.QuerySingleAsync<Machine>(query, new { name = dto.Name });
        machine.Malfunctions = new List<Malfunction>();
        machine.AverageRepairTime = 0;
        
        return machine;
    }
    
    public async Task<Machine?> UpdateAsync(int id, UpdateMachineDto dto)
    {
        const string query = @"
            UPDATE machines 
            SET name = @Name, updated_at = @UpdatedAt 
            WHERE id = @Id
            RETURNING id, name, created_at as CreatedAt, updated_at as UpdatedAt";

        var machine = await _dbConnection.QueryFirstOrDefaultAsync<Machine>(query, new { 
            Id = id, 
            Name = dto.Name,
            UpdatedAt = DateTime.UtcNow 
        });
    
        if (machine != null)
        {
            machine.Malfunctions = new List<Malfunction>();
            machine.AverageRepairTime = 0;
        }

        return machine;
    }

    //TODO: Soft delete
    public async Task<bool> DeleteAsync(int id)
    {
        const string query = "DELETE FROM machines WHERE id = @Id";
        var rowsAffected = await _dbConnection.ExecuteAsync(query, new { Id = id });
        return rowsAffected > 0;
    }
}

public interface IMachineRepository
{
    Task<IEnumerable<Machine>> GetAllAsync();
    Task<Machine?> GetByIdAsync(int id);
    Task<Machine> CreateAsync(CreateMachineDto dto);
    Task<Machine?> UpdateAsync(int id, UpdateMachineDto dto);
    Task<bool> DeleteAsync(int id);
}