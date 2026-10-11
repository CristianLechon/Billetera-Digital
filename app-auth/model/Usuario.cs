using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace app_auth.model;

[Table("usuarios")]
[Index(nameof(Identificacion), IsUnique = true)]
[Index(nameof(User), IsUnique = true)] 
public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public string Identificacion { get; set; } = null!;

    [Required, MaxLength(50)]
    public string User {get; set;} = null!;

    [Required, MaxLength(256)]
    public string PasswordHash {get; set;} = null!;
    
    [Required, MaxLength(30)]
    public string Role {get; set;} = null!;
}
