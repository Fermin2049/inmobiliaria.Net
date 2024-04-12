using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp.net.Models;

public class Pago
{
    [Key]
    public int PagoID { get; set; }
    [ForeignKey("Contrato")]
    public int ContratoID { get; set; }
    [Required]
    public int NroPago { get; set; }
    [Required]
    public DateTime FechaPago { get; set; }
    [Required, MaxLength(255)]
    public string? Detalle { get; set; }
    [Required]
    public decimal Importe { get; set; }
    [Required]
    public bool Estado { get; set; }

    public virtual Contrato Contrato { get; set; }
}