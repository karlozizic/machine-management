using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace MachineManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MachinesController : ControllerBase
{
    //TODO: Controller-Service-Repository
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public MachinesController(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
                            ?? "Host=localhost;Database=machine_management;Username=admin;Password=admin123";
    }

    [HttpGet]
    public async Task<IActionResult> GetMachines()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        
        const string sql = "SELECT id, name FROM machines ORDER BY name";
        
        var machines = await connection.QueryAsync(sql);
        
        return Ok(machines);
    }
}