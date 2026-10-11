using System.Security.Cryptography;
using app_wallet;
using app_wallet.service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Configurar la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConexion")));

// Leemos la llave publica para validar el Token
string publicKeyPath = builder.Configuration["JwtSettings:PublicKeyPath"] ?? "publicKey.pem";
string publicKeyContent = File.ReadAllText(publicKeyPath);
 
// validar los tokens que vayan llegando.
var rsa = RSA.Create();
rsa.ImportFromPem(publicKeyContent);
var securityKey = new RsaSecurityKey(rsa);

// CONFIGURAR LA AUTENTICACIÓN JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false; // conserva los nombres "sub" y "role" tal cual
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey
        };
    });

builder.Services.AddAuthorization();

// Inyección de dependencias de los servicios
builder.Services.AddScoped<TransaccionService>();

// Configuración general
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()));

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
app.UseAuthentication();
app.UseAuthorization();
 
app.MapControllers();

app.Run();
