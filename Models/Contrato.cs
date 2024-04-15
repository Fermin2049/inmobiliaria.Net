using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp.net.Models;



public class Contrato
{
    [Key]
    public int ContratoID { get; set; }
    [ForeignKey("Inmueble")]
    public int InmuebleID { get; set; }
    [ForeignKey("Inquilino")]
    public int InquilinoID { get; set; }
    [Required]
    public DateTime FechaInicio { get; set; }
    [Required]
    public DateTime FechaFin { get; set; }
    [Required]
    public int MontoRenta { get; set; }
    public int Deposito { get; set; }
    public int Comision { get; set; }
    [MaxLength(255)]
    public string? Condiciones { get; set; }

    public virtual Inmueble? Inmueble { get; set; }
    public virtual Inquilino? Inquilino { get; set; }
}