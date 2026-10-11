using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_wallet.model;

[Table("transacciones")]
public class Transaccion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    [Required]
    public TipoTransaccion Tipo { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Valor { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal SaldoDisponible { get; set; }

    [Required, MaxLength(20)]
    public string NumeroCuenta { get; set; } = null!;

    [ForeignKey("NumeroCuenta")]
    public Cuenta Cuenta { get; set; } = null!;

}
