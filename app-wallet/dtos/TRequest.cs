using System.ComponentModel.DataAnnotations;
using app_wallet.model;

namespace app_wallet.dtos;

public record class TRequest(decimal Valor, TipoTransaccion TipoTransaccion);
