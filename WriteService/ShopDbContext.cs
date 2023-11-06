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
            optionsBuilder.UseNpgsql("your_connection_string_here");
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
