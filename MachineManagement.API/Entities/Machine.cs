using MachineManagement.API.Models;

namespace MachineManagement.API.Entities;

public class Machine
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<Malfunction> Malfunctions { get; set; }
    public double AverageRepairTime { get; set; }

    public string AverageRepairTimeFormatted
    {
        get
        {
            if (AverageRepairTime <= 0)
                return "0";
            
            var hours = (int)AverageRepairTime;
            var minutes = (int)((AverageRepairTime - hours) * 60);
            return $"{hours}h {minutes}m";
        }
    }
}