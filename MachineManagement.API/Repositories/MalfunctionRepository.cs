using System.Data;
using Dapper;
using MachineManagement.API.Entities;
using MachineManagement.API.Models;

namespace MachineManagement.API.Repositories;

public class MalfunctionRepository : IMalfunctionRepository
{
    private readonly IDbConnection _dbConnection;
    
    public MalfunctionRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    
    public async Task<IEnumerable<Malfunction>> GetAllAsync()
    {
        const string query = @"
            SELECT 
                mf.id,
                mf.name,
                mf.machine_id as MachineId,
                m.name as MachineName,
                mf.priority,
                mf.start_time as StartTime,
                mf.end_time as EndTime,
                mf.description,
                mf.is_resolved as IsResolved,
                mf.created_at as CreatedAt,
                mf.updated_at as UpdatedAt
            FROM malfunctions mf
            INNER JOIN machines m ON mf.machine_id = m.id
            ORDER BY mf.start_time DESC
            ";
        
        var malfunctions = await _dbConnection.QueryAsync<Malfunction>(query);
        return malfunctions;
    }
    

    public async Task<Malfunction?> GetByIdAsync(int id)
    {
        const string query = @"
            SELECT 
                mf.id, 
                mf.name,
                mf.machine_id as MachineId,
                m.name as MachineName,
                mf.priority,
                mf.start_time as StartTime,
                mf.end_time as EndTime,
                mf.description,
                mf.is_resolved as IsResolved,
                mf.created_at as CreatedAt,
                mf.updated_at as UpdatedAt
            FROM malfunctions mf
            INNER JOIN machines m ON mf.machine_id = m.id
            WHERE mf.id = @Id";
        
        return await _dbConnection.QueryFirstOrDefaultAsync<Malfunction>(query, new { Id = id });
    }

    public async Task<PagedResult<Malfunction>> GetPagedAsync(int page, int pageSize)
    {
        int offset = (page - 1) * pageSize;

        const string countQuery = @"SELECT COUNT(*) FROM malfunctions";
        var totalCount = await _dbConnection.QuerySingleAsync<int>(countQuery);

        const string query = @"
            SELECT
                mf.id,
                mf.name,
                mf.machine_id as MachineId,
                m.name as MachineName,
                mf.priority,
                mf.start_time as StartTime,
                mf.end_time as EndTime,
                mf.description,
                mf.is_resolved as IsResolved,
                mf.created_at as CreatedAt,
                mf.updated_at as UpdatedAt
            FROM malfunctions mf
            INNER JOIN machines m ON mf.machine_id = m.id
            ORDER BY mf.priority ASC, mf.start_time DESC
            LIMIT @PageSize OFFSET @Offset";
        
        var malfunctions = await _dbConnection.QueryAsync<Malfunction>(query, new { PageSize = pageSize, Offset = offset });
        
        return new PagedResult<Malfunction>
        {
            Items = malfunctions,
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize
        };
    }
    
    public async Task<Malfunction> CreateAsync(CreateMalfunctionDto dto)
    {
        const string query = @"
            INSERT INTO malfunctions (name, machine_id, priority, description, start_time)
            VALUES (@Name, @MachineId, @Priority, @Description, @StartTime)
            RETURNING id, name, machine_id as MachineId, priority, start_time as StartTime,
                      end_time as EndTime, description, is_resolved as IsResolved, 
                      created_at as CreatedAt, updated_at as UpdatedAt";
        
        var malfunction = await _dbConnection.QuerySingleAsync<Malfunction>(query, new
        {
            Name = dto.Name,
            MachineId = dto.MachineId,
            Priority = (int)dto.Priority,
            Description = dto.Description,
            StartTime = DateTime.UtcNow
        });
        
        // dynamic because of name property which is not in Malfunction model
        var machine = await _dbConnection.QueryFirstAsync<dynamic>(
            "SELECT name FROM machines WHERE id = @Id", new { Id = dto.MachineId });
        malfunction.MachineName = machine.name;
        
        return malfunction;
    }

    public async Task<Malfunction?> UpdateAsync(int id, UpdateMalfunctionDto dto)
    {
        const string query = @"
            UPDATE malfunctions
            SET name = @Name, priority = @Priority,
                description = @Description, updated_at = @UpdatedAt
            WHERE id = @Id
            RETURNING id, name, machine_id as MachineId, priority, start_time as StartTime,
                        end_time as EndTime, description, is_resolved as IsResolved,
                        created_at as CreatedAt, updated_at as UpdatedAt";

        var malfunction = await _dbConnection.QuerySingleOrDefaultAsync<Malfunction>(query, new
        {
            Id = id,
            Name = dto.Name,
            Priority = (int)dto.Priority,
            Description = dto.Description,
            UpdatedAt = DateTime.UtcNow
        });
        
        if (malfunction == null)
            return null;
        
        // dynamic because of name property which is not in Malfunction model
        var machine = await _dbConnection.QueryFirstAsync<dynamic>(
            "SELECT name FROM machines WHERE id = @Id", new { Id = malfunction.MachineId });
        
        malfunction.MachineName = machine.name;
        
        return malfunction;
    }

    //TODO: Soft delete
    public async Task<bool> DeleteAsync(int id)
    {
        const string query = "DELETE FROM malfunctions WHERE id = @Id";
        var rowsAffected = await _dbConnection.ExecuteAsync(query, new { Id = id });
        return rowsAffected > 0;
    }

    public async Task<bool> Resolve(int id, 
        DateTime? endTime = null)
    {
        var endTimeValue = endTime ?? DateTime.UtcNow;

        const string query = @"
            UPDATE malfunctions
            SET is_resolved = TRUE,
                end_time = @EndTime,
                updated_at = @UpdatedAt
            WHERE id = @Id AND is_resolved = FALSE
            ";

        var rowsAffected = await _dbConnection.ExecuteAsync(query, new
        {
            Id = id,
            IsResolved = true,
            EndTime = endTimeValue,
            UpdatedAt = DateTime.UtcNow
        });
        
        return rowsAffected > 0;
    }   

    public async Task<IEnumerable<Malfunction>> GetByMachineIdAsync(int machineId)
    {
        const string query = @"
            SELECT id,
                name,
                machine_id as MachineId,
                priority,
                start_time as StartTime,
                end_time as EndTime,
                description,
                is_resolved as IsResolved,
                created_at as CreatedAt,
                updated_at as UpdatedAt
            FROM malfunctions
            WHERE machine_id = @MachineId
            ORDER BY start_time DESC";
        
        var malfunctions = await _dbConnection.QueryAsync<Malfunction>(query, new { MachineId = machineId });
        return malfunctions;
    }
}

public interface IMalfunctionRepository
{
    Task<IEnumerable<Malfunction>> GetAllAsync();
    Task<Malfunction?> GetByIdAsync(int id);
    Task<PagedResult<Malfunction>> GetPagedAsync(int page, int pageSize);
    Task<Malfunction> CreateAsync(CreateMalfunctionDto dto);
    Task<Malfunction?> UpdateAsync(int id, UpdateMalfunctionDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> Resolve(int id, DateTime? endTime = null);
    Task<IEnumerable<Malfunction>> GetByMachineIdAsync(int machineId);
}