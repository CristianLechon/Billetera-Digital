using System.Security.Cryptography;
using app_auth;
using app_auth.service;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConexion")));

// Configuración RSA (Leemos el archivo UNA sola vez al arrancar)
string keyPath = builder.Configuration["JwtSettings:PrivateKeyPath"] ?? "privateKey.pem";
var rsa = RSA.Create();
rsa.ImportFromPem(File.ReadAllText(keyPath));

// Guardamos las credenciales en memoria como Singleton
builder.Services.AddSingleton(new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256));

// Inyección de dependencias de los servicios
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddSingleton<JwtService>();

// Configuración general
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Cors
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Migraciones automáticas
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

app.Run();
