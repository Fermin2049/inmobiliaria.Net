using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp.net.Models;

public class Inmueble
{
    [Key]
    public int InmuebleID { get; set; }
    [ForeignKey("Propietario")]
    public int PropietarioID { get; set; }
    [Required, MaxLength(100)]
    public string DireccionInmueble { get; set; }
    [Required, MaxLength(25)]
    public string Uso { get; set; }
    [Required, MaxLength(50)]
    public string Tipo { get; set; }
    [Required]
    public int CantAmbiente { get; set; }
    [Required]
    public int Valor { get; set; }
    public bool Estado { get; set; }

    public virtual Propietario Propietario { get; set; }
}