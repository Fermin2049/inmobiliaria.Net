using System.ComponentModel.DataAnnotations;

namespace asp.net.Models;

public class Propietario
{
 
    [Key]
    public int PropietarioID { get; set; }
    [Required, MaxLength(50)]
    public string? Nombre { get; set; }
    [Required, MaxLength(50)]
    public string? Apellido { get; set; }
    [Required, MaxLength(100)]
    public string? Email { get; set; }
    [Required, MaxLength(50)]
    public string? Telefono { get; set; }
    [Required]
    public int Dni { get; set; }
    public bool Estado { get; set; }
    
}