
namespace MachineManagement.API.Entities;

public class Malfunction
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MachineId { get; set; }
    public string MachineName { get; set; }
    public Priority Priority { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Description { get; set; }
    public bool IsResolved { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}