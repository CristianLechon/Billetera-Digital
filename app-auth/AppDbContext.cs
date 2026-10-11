using System;
using app_auth.model;
using Microsoft.EntityFrameworkCore;

namespace app_auth;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<Usuario> Usuarios => Set<Usuario>();
}
