using WriteService.Entities;

namespace WriteService;

public static class Seeds
{
    public static void ApplyDatabaseSeeds(ShopDbContext dbContext)
    {
        dbContext.ProductCategories.AddRange(ProductCategories);
        dbContext.Vendors.AddRange(Vendors);
    }

    private static readonly List<ProductCategoryEntity> ProductCategories = new()
    {
        new ProductCategoryEntity
        {
            Name = "Electronics",
            Description = "Devices and gadgets including smartphones, laptops, and cameras"
        },
        new ProductCategoryEntity
        {
            Name = "Clothing",
            Description = "Apparel for men, women, and children in various styles and sizes"
        },
        new ProductCategoryEntity
        {
            Name = "Home Appliances",
            Description = "Essential appliances for home such as refrigerators, washing machines, and microwaves"
        },
        new ProductCategoryEntity
        {
            Name = "Books",
            Description = "A wide range of books from fiction to educational textbooks"
        },
        new ProductCategoryEntity
        {
            Name = "Fitness",
            Description = "Fitness equipment and accessories including weights, yoga mats, and treadmills"
        }
    };

    private static readonly List<VendorEntity> Vendors = new()
    {
        new VendorEntity
        {
            Name = "Tech Gadgets Inc.",
            Country = "USA",
            ZipCode = "10001",
            City = "New York",
            Street = "5th Avenue",
            HouseNumber = "101"
        },
        new VendorEntity
        {
            Name = "Fashion Forward",
            Country = "France",
            ZipCode = "75008",
            City = "Paris",
            Street = "Champs-Élysées",
            HouseNumber = "70"
        },
        new VendorEntity
        {
            Name = "Home Essentials Ltd.",
            Country = "UK",
            ZipCode = "W1A 1AA",
            City = "London",
            Street = "Oxford Street",
            HouseNumber = "200"
        },
        new VendorEntity
        {
            Name = "Book World",
            Country = "Canada",
            ZipCode = "M5V 1K4",
            City = "Toronto",
            Street = "Bay Street",
            HouseNumber = "500"
        },
        new VendorEntity
        {
            Name = "Fitness Pro Shop",
            Country = "Australia",
            ZipCode = "2000",
            City = "Sydney",
            Street = "George Street",
            HouseNumber = "250"
        }
    };
}