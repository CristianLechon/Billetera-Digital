using app_wallet.dtos;
using app_wallet.model;
using Microsoft.EntityFrameworkCore;

namespace app_wallet.service;

public class TransaccionService
{
    private readonly AppDbContext context;

    public TransaccionService(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<CuentaDto?> ConsultarSaldo(string numeroCuenta)
    {
        return await context.Cuentas
            .AsNoTracking()
            .Where(c => c.NumeroCuenta == numeroCuenta)
            .Select(c => new CuentaDto(c.NumeroCuenta, c.Saldo))
            .FirstOrDefaultAsync();
    }

    public async Task<List<TransaccionDto>> ConsultarMovimientos(string numeroCuenta, DateTime fechaInicio, DateTime fechaFin)
    {
        return await context.Transacciones
            .AsNoTracking()
            .Where(m => m.NumeroCuenta == numeroCuenta && m.Fecha.Date >= fechaInicio.Date && m.Fecha.Date <= fechaFin.Date)
            .OrderByDescending(m => m.Fecha)
            .Select(m => new TransaccionDto(m.Fecha, m.Tipo, m.Valor, m.SaldoDisponible))
            .ToListAsync();
    }

    public async Task<bool> RegistrarTransaccion(string numeroCuenta, TRequest request)
    {
        bool exito = false;
        int intentos = 0;
        int maxIntentos = 3;

        while (!exito && intentos < maxIntentos)
        {
            intentos++;

            try
            {
                var cuenta = await context.Cuentas.FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
                if (cuenta == null)
                {
                    return false;
                }

                bool esDebito = request.TipoTransaccion == TipoTransaccion.Debito;
                
                // No permitir saldo negativo
                if (esDebito && (cuenta.Saldo - request.Valor) < 0)
                {
                    throw new InvalidOperationException("Saldo insuficiente. La transacción no permitida.");
                }

                cuenta.Saldo = esDebito ? (cuenta.Saldo - request.Valor) : (cuenta.Saldo + request.Valor);

                var transaccion = new Transaccion
                {
                    Fecha = DateTime.UtcNow, // Guardamos siempre en UTC para evitar problemas de zonas horarias
                    Tipo = request.TipoTransaccion,
                    Valor = request.Valor,
                    SaldoDisponible = cuenta.Saldo,
                    NumeroCuenta = numeroCuenta
                };

                context.Transacciones.Add(transaccion);

                await context.SaveChangesAsync();
                exito = true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (intentos >= maxIntentos)
                {
                    throw new Exception("El sistema está experimentando alta concurrencia. Por favor, intente de nuevo.");
                }

                // Recargar los datos actualizados para el siguiente intento
                foreach (var entry in context.ChangeTracker.Entries())
                {
                    await entry.ReloadAsync();
                }
            }
        }

        return exito;
    }
}
