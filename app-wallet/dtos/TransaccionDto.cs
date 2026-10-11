using app_wallet.model;

namespace app_wallet.dtos;

public record TransaccionDto(DateTime Fecha, TipoTransaccion TipoTransaccion, decimal Valor, decimal SaldoDisponible);
