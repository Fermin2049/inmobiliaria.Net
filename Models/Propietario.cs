using System.ComponentModel.DataAnnotations;

namespace asp.net.Models;

public class Propietario
{
 
    [Key]
    public int PropietarioID { get; set; }
    [Required]
    public string? Nombre { get; set; }
    [Required]
    public string? Apellido { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Telefono { get; set; }
    [Required]
    public int Dni { get; set; }
    
}