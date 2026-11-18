
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

        // Model Configuration
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
        }
    }
}
