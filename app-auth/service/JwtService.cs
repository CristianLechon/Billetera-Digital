using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace app_auth.service;

public class JwtService
{
    private readonly IConfiguration configuration;
    private readonly SigningCredentials credentials; // Llegan desde Program.cs

    public JwtService(IConfiguration configuration, SigningCredentials credentials)
    {
        this.configuration = configuration;
        this.credentials = credentials; // Se inyecta la instancia única
    }

    public TokenResponse GenerarToken(string Identificacion)
    {
        // Leemos el appsettings.json
        double segundos = double.Parse(configuration["JwtSettings:ExpirationSeconds"]!);
        DateTime fechaExpiracion = DateTime.UtcNow.AddSeconds(segundos);

        var issuer = configuration.GetValue<string>("JwtSettings:Issuer") ?? "vuelo-auth";

        var claims = new []
        {
            new Claim(JwtRegisteredClaimNames.Sub, Identificacion)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            claims: claims,
            expires: fechaExpiracion,
            signingCredentials: credentials
        );

        var jwtString = new JwtSecurityTokenHandler().WriteToken(token);
        long expiracionUnix = new DateTimeOffset(fechaExpiracion).ToUnixTimeSeconds();

        // Retornamos el DTO
        return new TokenResponse(jwtString, expiracionUnix);
    }

    public record TokenResponse(string AccessToken, long ExpiresAt);
}
