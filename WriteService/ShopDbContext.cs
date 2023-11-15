using Microsoft.EntityFrameworkCore;
using WriteService.Entities;

namespace WriteService;

public class ShopDbContext : DbContext
{
    public ShopDbContext()
    {
    }

    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VendorEntity>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<ProductEntity>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<OrderEntity>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<CustomerEntity>()
            .HasQueryFilter(e => !e.IsDeleted);

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
    }

#nullable disable

    public DbSet<CustomerEntity> Customers { get; init; }
    public DbSet<OrderEntity> Orders { get; init; }
    public DbSet<ProductEntity> Products { get; init; }
    public DbSet<ReviewEntity> ProductReviews { get; init; }
    public DbSet<ProductCategoryEntity> ProductCategories { get; init; }
    public DbSet<ProductSubCategoryEntity> ProductSubCategories { get; init; }
    public DbSet<VendorEntity> Vendors { get; init; }
}