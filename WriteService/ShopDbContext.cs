using Microsoft.EntityFrameworkCore;
using WriteService.Entities;

namespace WriteService
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string host = "postgres"; // Use the service name as the host
            string database = "cqrs";
            string username = "root";
            string password = "toor";
            string port = "5432"; // PostgreSQL default port

            string connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};";

            optionsBuilder.UseNpgsql(connectionString);
        }

        public DbSet<CustomerEntity> Customers { get; set; } = null!;
        public DbSet<OrderEntity> Orders { get; set; } = null!;
        public DbSet<ProductCategoryEntity> ProductCategories { get; set; } = null!;
        public DbSet<ProductEntity> Products { get; set; } = null!;
        public DbSet<ProductReviewEntity> ProductReviews { get; set; } = null!;
        public DbSet<ProductSubCategoryEntity> ProductSubCategories { get; set; } = null!;
        public DbSet<VendorEntity> Vendors { get; set; } = null!;

        //TODO... define relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<VendorEntity>();



        }
    }
}
