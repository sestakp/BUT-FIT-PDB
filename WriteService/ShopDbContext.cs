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

        modelBuilder.Entity<ProductEntity>()
            .HasMany(e => e.Categories)
            .WithMany()
            .UsingEntity(
                "ProductCategoriesJoinTable",
                l => l.HasOne(typeof(ProductCategoryEntity))
                    .WithMany()
                    .HasForeignKey("CategoryId"),
                r => r.HasOne(typeof(ProductEntity))
                    .WithMany()
                    .HasForeignKey("ProductId"));

        modelBuilder.Entity<ProductEntity>()
            .HasMany(e => e.SubCategories)
            .WithMany()
            .UsingEntity(
                "ProductSubCategoriesJoinTable",
                l => l.HasOne(typeof(ProductSubCategoryEntity))
                    .WithMany()
                    .HasForeignKey("SubCategoryId"),
                r => r.HasOne(typeof(ProductEntity))
                    .WithMany()
                    .HasForeignKey("ProductId"));

        modelBuilder.Entity<OrderEntity>()
            .HasMany(e => e.Products)
            .WithMany()
            .UsingEntity(
                "OrderProductsJoinTable",
                l => l.HasOne(typeof(ProductEntity))
                    .WithMany()
                    .HasForeignKey("ProductId"),
                r => r.HasOne(typeof(OrderEntity))
                    .WithMany()
                    .HasForeignKey("OrderId"));

        modelBuilder.Entity<ReviewEntity>()
            .HasOne<CustomerEntity>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId);

        modelBuilder.Entity<ReviewEntity>()
            .HasOne<ProductEntity>()
            .WithMany()
            .HasForeignKey(x => x.ProductId);


        modelBuilder.Entity<AddressEntity>()
            .HasOne<CustomerEntity>()
            .WithMany()
            .HasForeignKey(a => a.CustomerId);

        modelBuilder.Entity<CustomerEntity>()
            .HasMany<AddressEntity>()
            .WithOne()
            .HasForeignKey(a => a.CustomerId);


        modelBuilder.Entity<ProductCategoryEntity>()
            .HasMany<ProductSubCategoryEntity>()
            .WithOne()
            .HasForeignKey(c => c.CategoryId);


        modelBuilder.Entity<ProductSubCategoryEntity>()
            .HasOne<ProductCategoryEntity>()
            .WithMany()
            .HasForeignKey(c => c.CategoryId);
    }

#nullable disable

    public DbSet<CustomerEntity> Customers { get; init; }
    public DbSet<OrderEntity> Orders { get; init; }
    public DbSet<ProductEntity> Products { get; init; }
    public DbSet<ReviewEntity> ProductReviews { get; init; }
    public DbSet<ProductCategoryEntity> ProductCategories { get; init; }
    public DbSet<ProductSubCategoryEntity> ProductSubCategories { get; init; }
    public DbSet<VendorEntity> Vendors { get; init; }
    public DbSet<AddressEntity> Addresses { get; init; }
}