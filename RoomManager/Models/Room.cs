using System.ComponentModel.DataAnnotations;

namespace RoomManager.Models;

public class Room
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string BuildingCode { get; set; }
    
    public int Floor { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "You must select a floor")];
    public int Capacity { get; set; }
    
    public bool HasProjector { get; set; }
    
    public bool IsActive { get; set; }
}