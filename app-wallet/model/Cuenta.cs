using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_wallet.model;

[Table("cuentas")]
public class Cuenta
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [MaxLength(20)]
    public string NumeroCuenta { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Saldo { get; set; }

    // Este campo maneja la concurrencia.
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;
}
