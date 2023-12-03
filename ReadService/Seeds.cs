using MongoDB.Driver;
using ReadService.Data;
using ReadService.Data.Models;

namespace ReadService;

public static class Seeds
{
    public static void ApplyDatabaseSeeds(IMongoDatabase database)
    {
        database.Collection<Category>().InsertMany(Categories);
        database.Collection<ProductsOfCategory>().InsertMany(ProductsOfCategory);
        database.Collection<ProductsOfSubCategory>().InsertMany(ProductsOfSubCategory);
    }

    private static readonly List<ProductsOfCategory> ProductsOfCategory = new()
    {
        new()
        {
            CategoryName = "Electronics",
            CategoryNormalizedName = "electronics",
            Products = new List<ProductsOfCategory.Product>()
        },
        new()
        {
            CategoryName = "Clothing",
            CategoryNormalizedName = "clothing",
            Products = new List<ProductsOfCategory.Product>()
        },
        new()
        {
            CategoryName = "Home Appliances",
            CategoryNormalizedName = "home-appliances",
            Products = new List<ProductsOfCategory.Product>()
        },
        new()
        {
            CategoryName = "Books",
            CategoryNormalizedName = "books",
            Products = new List<ProductsOfCategory.Product>()
        },
        new()
        {
            CategoryName = "Fitness",
            CategoryNormalizedName = "fitness",
            Products = new List<ProductsOfCategory.Product>()
        }
    };

    private static readonly List<ProductsOfSubCategory> ProductsOfSubCategory = new()
    {
        new()
        {
            CategoryName = "Electronics",
            CategoryNormalizedName = "electronics",
            SubCategoryName = "Smartphones",
            SubCategoryNormalizedName = "smartphones",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Electronics",
            CategoryNormalizedName = "electronics",
            SubCategoryName = "Laptops",
            SubCategoryNormalizedName = "laptops",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Electronics",
            CategoryNormalizedName = "electronics",
            SubCategoryName = "Cameras",
            SubCategoryNormalizedName = "cameras",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Clothing",
            CategoryNormalizedName = "clothing",
            SubCategoryName = "Men's Wear",
            SubCategoryNormalizedName = "mens-wear",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Clothing",
            CategoryNormalizedName = "clothing",
            SubCategoryName = "Women's Wear",
            SubCategoryNormalizedName = "womens-wear",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Clothing",
            CategoryNormalizedName = "clothing",
            SubCategoryName = "Children's Wear",
            SubCategoryNormalizedName = "childrens-wear",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Home Appliances",
            CategoryNormalizedName = "home-appliances",
            SubCategoryName = "Kitchen Appliances",
            SubCategoryNormalizedName = "kitchen-appliances",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Home Appliances",
            CategoryNormalizedName = "home-appliances",
            SubCategoryName = "Laundry Appliances",
            SubCategoryNormalizedName = "laundry-appliances",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Home Appliances",
            CategoryNormalizedName = "home-appliances",
            SubCategoryName = "Small Appliances",
            SubCategoryNormalizedName = "small-appliances",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Books",
            CategoryNormalizedName = "books",
            SubCategoryName = "Fiction",
            SubCategoryNormalizedName = "fiction",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Books",
            CategoryNormalizedName = "books",
            SubCategoryName = "Non-Fiction",
            SubCategoryNormalizedName = "non-fiction",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Books",
            CategoryNormalizedName = "books",
            SubCategoryName = "Textbooks",
            SubCategoryNormalizedName = "textbooks",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Books",
            CategoryNormalizedName = "books",
            SubCategoryName = "Children's Books",
            SubCategoryNormalizedName = "childrens-books",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Fitness",
            CategoryNormalizedName = "fitness",
            SubCategoryName = "Gym",
            SubCategoryNormalizedName = "gym",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Fitness",
            CategoryNormalizedName = "fitness",
            SubCategoryName = "Yoga",
            SubCategoryNormalizedName = "yoga",
            Products = new List<ProductsOfSubCategory.Product>()
        },
        new()
        {
            CategoryName = "Fitness",
            CategoryNormalizedName = "fitness",
            SubCategoryName = "Outdoor",
            SubCategoryNormalizedName = "outdoor",
            Products = new List<ProductsOfSubCategory.Product>()
        }
    };

    private static readonly List<Category> Categories = new()
    {
        new Category()
        {
            Name = "Electronics",
            NormalizedName = "electronics",
            Description = "Devices and gadgets including smartphones, laptops, and cameras",
            SubCategories = new List<SubCategory>()
            {
                new()
                {
                    Name = "Smartphones",
                    Description = "Latest and advanced smartphones",
                    NormalizedName = "smartphones"
                },
                new()
                {
                    Name = "Laptops",
                    Description = "High-performance and portable laptops",
                    NormalizedName = "laptops"
                },
                new()
                {
                    Name = "Cameras",
                    Description = "Digital cameras for professional photography",
                    NormalizedName = "cameras"
                }
            }
        },
        new Category()
        {
            Name = "Clothing",
            Description = "Apparel for men, women, and children in various styles and sizes",
            NormalizedName = "clothing",
            SubCategories = new List<SubCategory>()
            {
                new()
                {
                    Name = "Men's Wear",
                    Description = "Stylish and comfortable clothing for men",
                    NormalizedName = "mens-wear",
                },
                new()
                {
                    Name = "Women's Wear",
                    Description = "Fashionable women's clothing for all occasions",
                    NormalizedName = "womens-wear"
                },
                new()
                {
                    Name = "Children's Wear",
                    Description = "Durable and cute clothing for children",
                    NormalizedName = "childrens-wear"
                }
            }
        },
        new Category()
        {
            Name = "Home Appliances",
            Description = "Essential appliances for home such as refrigerators, washing machines, and microwaves",
            NormalizedName = "home-appliances",
            SubCategories = new List<SubCategory>
            {
                new()
                {
                    Name = "Kitchen Appliances",
                    Description = "Appliances for kitchen use, like microwaves, ovens, and toasters",
                    NormalizedName = "kitchen-appliances"
                },
                new()
                {
                    Name = "Laundry Appliances",
                    Description = "Appliances for laundry, including washing machines and dryers",
                    NormalizedName = "laundry-appliances"
                },
                new()
                {
                    Name = "Small Appliances",
                    Description = "Small household appliances like blenders, coffee makers, and irons",
                    NormalizedName = "small-appliances"
                }
            }
        },
        new Category()
        {
            Name = "Books",
            Description = "A wide range of books from fiction to educational textbooks",
            NormalizedName = "books",
            SubCategories = new List<SubCategory>
            {
                new()
                {
                    Name = "Fiction",
                    Description = "Novels and stories spanning various genres",
                    NormalizedName = "fiction"
                },
                new()
                {
                    Name = "Non-Fiction",
                    Description = "Books covering real-life subjects and events",
                    NormalizedName = "non-fiction"
                },
                new()
                {
                    Name = "Textbooks",
                    Description = "Educational textbooks for different subjects and levels",
                    NormalizedName = "textbooks"
                },
                new()
                {
                    Name = "Children's Books",
                    Description = "Books for children including picture books and early reading material",
                    NormalizedName = "childrens-books"
                }
            }
        },
        new Category()
        {
            Name = "Fitness",
            Description = "Fitness equipment and accessories including weights, yoga mats, and treadmills",
            NormalizedName = "fitness",
            SubCategories = new List<SubCategory>
            {
                new()
                {
                    Name = "Gym",
                    Description = "Equipment for gym workouts like weights and machines",
                    NormalizedName = "gym"
                },
                new()
                {
                    Name = "Yoga",
                    Description = "Accessories for yoga, including mats, blocks, and straps",
                    NormalizedName = "yoga"
                },
                new()
                {
                    Name = "Outdoor",
                    Description = "Equipment for outdoor fitness activities, such as running gear and bicycles",
                    NormalizedName = "outdoor"
                }
            }
        }
    };
}