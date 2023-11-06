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
            string host = "postgres";
            #if DEBUG
                host = "localhost";
            #endif
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
        public DbSet<ReviewEntity> ProductReviews { get; set; } = null!;
        public DbSet<ProductSubCategoryEntity> ProductSubCategories { get; set; } = null!;
        public DbSet<VendorEntity> Vendors { get; set; } = null!;
        public DbSet<AddressEntity> Addresses { get; set; } = null!;

        //TODO... define relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<VendorEntity>()
                .HasQueryFilter(e => !e.isDeleted);


            modelBuilder.Entity<ProductEntity>()
                .HasQueryFilter(e => !e.isDeleted);


            modelBuilder.Entity<OrderEntity>()
                .HasQueryFilter(e => !e.isDeleted);


            modelBuilder.Entity<CustomerEntity>()
                .HasQueryFilter(e => !e.isDeleted);

            modelBuilder.Entity<VendorEntity>()
                .HasMany(e => e.Products)
                .WithOne(e => e.Vendor)
                .HasForeignKey(e => e.VendorId);

            modelBuilder.Entity<ProductEntity>()
                .HasOne(e => e.Vendor)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.VendorId);

            modelBuilder.Entity<ProductEntity>()
                .HasMany(e => e.Reviews)
                .WithOne(e => e.Product)
                .HasForeignKey(e => e.ProductId);


            modelBuilder.Entity<ReviewEntity>()
                .HasOne(e => e.Product)
                .WithMany(e => e.Reviews)
                .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<ProductEntity>()
                .HasMany(e => e.Orders)
                .WithMany(e => e.Products);

            modelBuilder.Entity<OrderEntity>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Orders);

            modelBuilder.Entity<OrderEntity>()
                .HasOne(e => e.Customer)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.CustomerId);

            modelBuilder.Entity<CustomerEntity>()
                .HasMany(e => e.Orders)
                .WithOne(e => e.Customer)
                .HasForeignKey(e => e.CustomerId);

            modelBuilder.Entity<CustomerEntity>()
                .HasMany(e => e.Addresses)
                .WithOne(e => e.Customer)
                .HasForeignKey(e => e.CustomerId);

            modelBuilder.Entity<AddressEntity>()
                .HasOne(e => e.Customer)
                .WithMany(e => e.Addresses)
                .HasForeignKey(e => e.CustomerId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
