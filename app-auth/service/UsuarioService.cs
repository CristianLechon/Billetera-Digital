using System;
using app_auth.dtos;
using app_auth.Migrations;
using app_auth.model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace app_auth.service;

public class UsuarioService
{
    private readonly AppDbContext context;
    private readonly PasswordHasher<Usuario> hasher = new();

    public UsuarioService(AppDbContext context)
    {
        this.context = context;
    }

    // Listar todos los Usuarios.
    public async Task<List<UsuarioDto>> FindAllAsync()
    {
        return await context.Usuarios
        .AsNoTracking()
        .Select(u => new UsuarioDto(u.Id, u.User, u.Identificacion, u.Role))
        .ToListAsync();
    }

    // Validar el usuario y la contraseña.
    public async Task<UsuarioDto?> ValidarCredenciales(string User, string Password)
    {
        var usuario = await context.Usuarios
        .AsNoTracking()
        .Where(u => u.User == User)
        .FirstOrDefaultAsync();

        if (usuario == null)
        {
            Console.WriteLine("Usuario no encontrado");
            return null;
        }

        var resultado = hasher.VerifyHashedPassword(usuario, usuario.PasswordHash, Password);
        if (resultado == PasswordVerificationResult.Failed)
        {
            Console.WriteLine("Contraseña Incorrecta");
            return null;
        }
        return new UsuarioDto(usuario.Id, usuario.Identificacion, usuario.User, usuario.Role);
    }
}
