namespace MachineManagement.API.Models;

public class Machine
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<Malfunction> Malfunctions { get; set; }
    public double AverageRepairTime { get; set; }
}