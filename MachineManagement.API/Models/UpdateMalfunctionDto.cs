namespace MachineManagement.API.Models;

public class UpdateMalfunctionDto
{
    public string Name { get; set; } = string.Empty;
    public Priority Priority { get; set; } = Priority.Low;
    public string Description { get; set; } = string.Empty;
}