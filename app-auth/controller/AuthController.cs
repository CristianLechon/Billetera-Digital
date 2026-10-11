using app_auth.service;
using Microsoft.AspNetCore.Mvc;

namespace app_auth.controller;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UsuarioService usuarioService;
    private readonly JwtService jwtService;

    public AuthController(UsuarioService usuarioService, JwtService jwtService)
    {
        this.usuarioService = usuarioService;
        this.jwtService = jwtService;
    }

    [HttpPost("token")]
    public async Task<IActionResult> Token([FromBody] LoginRequest request)
    {
        var usuario = await usuarioService.ValidarCredenciales(request.User, request.Password);

        if (usuario == null)
        {
            return Unauthorized(); // retorno 401
        }

        // Genero el Token y envio la Identificacion
        var jwtResult = jwtService.GenerarToken(usuario.Identificacion);

        // Retornamos 200 y el objeto en JSON.
        return Ok(new TokenResponse(jwtResult.AccessToken, jwtResult.ExpiresAt, usuario.Role));
    }

    public record LoginRequest(string User, string Password);

    // Clase Auxiliar para la respuesta.
    public record TokenResponse(string AccessToken, long ExpiresAt, string Role);
}
