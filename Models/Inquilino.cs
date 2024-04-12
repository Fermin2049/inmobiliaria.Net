using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp.net.Models;


public class Inquilino
{
    [Key]
    public int InquilinoID { get; set; }
    [Required, MaxLength(50)]
    public string? Nombre { get; set; }
    [Required, MaxLength(50)]
    public string? Apellido { get; set; }
    [Required]
    public int Dni { get; set; }
    [Required, MaxLength(50)]
    public string? Telefono { get; set; }
    [Required, MaxLength(100)]
    public string? Email { get; set; }
    public bool Estado { get; set; }
}