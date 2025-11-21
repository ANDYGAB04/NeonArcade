using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NeonArcade.Server.Models;
using System.Text.Json;

namespace NeonArcade.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        override protected void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            modelbuilder.Entity<Order>(entity =>
            {
                entity.HasMany(o => o.OrderItems)
                       .WithOne(oi => oi.Order)
                       .HasForeignKey(oi => oi.OrderId);
                entity.Property(o=>o.TotalAmount).HasPrecision(18,2);
                entity.Property(o => o.OrderNumber).IsRequired();
                entity.Property(o => o.OrderNumber).HasMaxLength(50);
                entity.HasIndex(o => o.OrderNumber).IsUnique();
            });

            modelbuilder.Entity<User>(entity => {
            entity.HasMany(u => u.Orders)
                  .WithOne(o => o.User)
                  .HasForeignKey(o => o.UserId);
            entity.HasMany(u => u.CartItems)
                  .WithOne(ci => ci.User)
                  .HasForeignKey(ci => ci.UserId);


            });

            modelbuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(oi => oi.Price).HasPrecision(18, 2);
                entity.Property(oi => oi.SubTotal).HasPrecision(18, 2);

            });

            modelbuilder.Entity<CartItem>(entity =>
            {
                entity.Property(ci => ci.Price).HasPrecision(18, 2);
                entity.Property(ci => ci.SubTotal).HasPrecision(18, 2);
                entity.HasIndex(ci => new { ci.UserId, ci.GameId }).IsUnique();
            });

            modelbuilder.Entity<Game>(entity =>
            {
                entity.Property(oi => oi.Genres)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                          v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());
                entity.Property(oi => oi.Platforms)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                          v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());
                entity.Property(oi => oi.Tags)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                          v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());
                entity.Property(oi => oi.ScreenshotUrls)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                          v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());
                entity.Property(g => g.Developer).HasMaxLength(100);
                entity.Property(g => g.CoverImageUrl).HasMaxLength(1000);
                entity.Property(g => g.CoverImageUrl).IsRequired();
                entity.Property(g => g.Price).HasPrecision(18, 2);
                entity.Property(g => g.Title).IsRequired();
                entity.Property(g => g.Title).HasMaxLength(100);
                entity.Property(g => g.Description).HasMaxLength(1000);
                entity.Property(g => g.DiscountPrice).HasPrecision(18, 2);
                entity.HasMany(g => g.OrderItems)
                      .WithOne(oi => oi.Game)
                      .HasForeignKey(oi => oi.GameId);
                entity.HasMany(g => g.CartItems)
                        .WithOne(ci => ci.Game)
                        .HasForeignKey(ci => ci.GameId);
            });

        }
        public DbSet<Game> Games { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}
