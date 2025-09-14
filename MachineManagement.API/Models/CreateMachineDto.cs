using System.ComponentModel.DataAnnotations;

namespace MachineManagement.API.Models;

public class CreateMachineDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters.")]
    public string Name { get; set; } = string.Empty;
}