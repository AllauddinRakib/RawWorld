namespace RawWorld.API.Data;

using Microsoft.EntityFrameworkCore;
using RawWorld.API.Models;
using System.Reflection.Emit;

public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder m)
    {
        m.Entity<CartItem>()
         .HasIndex(c => new { c.UserId, c.ProductId })
         .IsUnique();
    }
}