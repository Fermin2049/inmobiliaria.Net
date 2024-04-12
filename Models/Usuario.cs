using System.ComponentModel.DataAnnotations;

namespace asp.net.Models;

public class Usuario
{
    [Key]
    public int UsuarioID { get; set; }
    [Required, MaxLength(50)]
    public string? Email { get; set; }
    [Required, MaxLength(50)]
    public string? Contrasena { get; set; }
    [Required, MaxLength(50)]
    public string? Rol { get; set; }
    public byte[]? Avatar { get; set; }
}