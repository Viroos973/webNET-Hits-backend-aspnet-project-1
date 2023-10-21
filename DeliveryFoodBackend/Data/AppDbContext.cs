using DeliveryFoodBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryFoodBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Basket> Baskets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>()
                .HasIndex(x => new { x.DishId, x.UserId })
                .IsUnique();

            modelBuilder.Entity<Basket>(options =>
            {
                options.HasIndex(x => new { x.UserId, x.OrderId })
                .IsUnique();

                options.Property(x => x.OrderId)
                .IsRequired(false);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
