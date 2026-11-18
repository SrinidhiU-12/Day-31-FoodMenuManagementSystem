
using FoodMenuManagement.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodMenuManagement.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<FoodItem> FoodItems { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        public DbSet<Payment> Payments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Order - TotalAmount precision
            builder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(10, 2);

            // Order - ApplicationUser relationship
            builder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderItem - UnitPrice precision
            builder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(10, 2);

            // OrderItem - SubTotal precision
            builder.Entity<OrderItem>()
                .Property(oi => oi.SubTotal)
                .HasPrecision(10, 2);

            // Order - OrderItem relationship
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // FoodItem - OrderItem relationship
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.FoodItem)
                .WithMany(f => f.OrderItems)
                .HasForeignKey(oi => oi.FoodItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment - Amount precision
            builder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(10, 2);

            // Order - Payment one-to-one relationship
            builder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // One payment record per order
            builder.Entity<Payment>()
                .HasIndex(p => p.OrderId)
                .IsUnique();
        }
    }
}
