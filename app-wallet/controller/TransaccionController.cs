using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using app_wallet.dtos;
using app_wallet.service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app_wallet.controller;

[ApiController]
[Route("transacciones")]
[Authorize]
public class TransaccionController : ControllerBase
{
    private readonly TransaccionService transaccionService;

    public TransaccionController(TransaccionService transaccionService)
    {
        this.transaccionService = transaccionService;
    }

    private string ObtenerCuentaDelToken()
    {
        var cuentaId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(cuentaId))
            throw new UnauthorizedAccessException("El token no contiene una identificación válida");

        return cuentaId;
    }

    [HttpGet("saldo")]
    public async Task<IActionResult> Saldo()
    {
        string numeroCuenta = ObtenerCuentaDelToken();
        var saldo = await transaccionService.ConsultarSaldo(numeroCuenta);

        if (saldo == null)
        {
            return NotFound("Cuenta no encontrada");
        }
        return Ok(saldo);
    }

    [HttpGet("movimientos")]
    public async Task<IActionResult> Movimientos([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
    {
        string numeroCuenta = ObtenerCuentaDelToken();
        var historial = await transaccionService.ConsultarMovimientos(numeroCuenta, fechaInicio, fechaFin);

        return Ok(historial);
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] TRequest request)
    {
        try
        {
            if (request.Valor <= 0)
                return BadRequest(new { error = "El valor debe ser mayor a 0" });

            string numeroCuenta = ObtenerCuentaDelToken();

            bool exito = await transaccionService.RegistrarTransaccion(numeroCuenta, request);

            if (!exito)
            {
                return NotFound(new { error = "Cuenta no encontrada" });
            }

            return Ok(new { mensaje = "Transacción registrada exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            // Captura el error de saldo negativo
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Captura el error de concurrencia máxima u otros errores de servidor
            return StatusCode(500, new { error = ex.Message });
        }
    }

}
