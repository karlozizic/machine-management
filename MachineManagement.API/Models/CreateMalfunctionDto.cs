using System.ComponentModel.DataAnnotations;
using MachineManagement.API.Entities;

namespace MachineManagement.API.Models;

public class CreateMalfunctionDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters.")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "MachineId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "MachineId must be a positive integer.")]
    public int MachineId { get; set; }
    public Priority Priority { get; set; } = Priority.Low;
    
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string Description { get; set; } = string.Empty;
}