using app_wallet.model;
using Microsoft.EntityFrameworkCore;

namespace app_wallet;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Cuenta> Cuentas => Set<Cuenta>();
    public DbSet<Transaccion> Transacciones => Set<Transaccion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Guardar el enum como texto en la base de datos
        modelBuilder.Entity<Transaccion>()
            .Property(m => m.Tipo)
            .HasConversion<string>()
            .HasMaxLength(10);
    }
}