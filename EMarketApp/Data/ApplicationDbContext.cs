using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EMarketApp.Models;
using Microsoft.AspNetCore.Identity;

namespace EMarketApp.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("EMarketApp");
        base.OnModelCreating(modelBuilder);

        // Define Table
        modelBuilder.Entity<Category>().ToTable("Category");
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<CartItem>().ToTable("CartItems");
        modelBuilder.Entity<Order>().ToTable("Orders");
        modelBuilder.Entity<OrderItem>().ToTable("OrderItems");

        // Define Primary Key
        modelBuilder.Entity<Category>()
            .HasKey(c => c.CategoryId);
        modelBuilder.Entity<Product>()
            .HasKey(p => p.ProductId);
        modelBuilder.Entity<CartItem>()
            .HasKey(ci => ci.CartItemId);
        modelBuilder.Entity<Order>()
            .HasKey(o => o.OrderId);
        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => oi.OrderItemId);
    }
}