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
                "ProductCategories",
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
                "ProductSubCategories",
                l => l.HasOne(typeof(ProductSubCategoryEntity))
                    .WithMany()
                    .HasForeignKey("SubCategoryId"),
                r => r.HasOne(typeof(ProductEntity))
                    .WithMany()
                    .HasForeignKey("ProductId"));

        modelBuilder.Entity<OrderProductEntity>()
            .HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId);

        modelBuilder.Entity<OrderProductEntity>()
            .HasOne(x => x.Order)
            .WithMany(x => x.OrderProducts)
            .HasForeignKey(x => x.OrderId);

        modelBuilder.Entity<OrderProductEntity>()
            .ToTable("OrderProducts");
    }

#nullable disable

    public DbSet<CustomerEntity> Customers { get; init; }
    public DbSet<OrderEntity> Orders { get; init; }
    public DbSet<ProductEntity> Products { get; init; }
    public DbSet<ReviewEntity> Reviews { get; init; }
    public DbSet<ProductCategoryEntity> Categories { get; init; }
    public DbSet<ProductSubCategoryEntity> SubCategories { get; init; }
    public DbSet<VendorEntity> Vendors { get; init; }
    public DbSet<AddressEntity> Addresses { get; init; }
}