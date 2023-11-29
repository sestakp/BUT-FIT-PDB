using MongoDB.Driver;
using ReadService.Data;
using ReadService.Data.Models;

namespace ReadService;

public static class Seeds
{
    public static void ApplyDatabaseSeeds(IMongoDatabase database)
    {
        database.Collection<Category>().InsertMany(Categories);
    }

    private static readonly List<Category> Categories = new()
    {
        new Category()
        {
            Name = "Electronics",
            Description = "Devices and gadgets including smartphones, laptops, and cameras",
            SubCategories = new List<SubCategory>()
            {
                new()
                {
                    Name = "Smartphones",
                    Description = "Latest and advanced smartphones"
                },
                new()
                {
                    Name = "Laptops",
                    Description = "High-performance and portable laptops"
                },
                new()
                {
                    Name = "Cameras",
                    Description = "Digital cameras for professional photography"
                }
            }
        },
        new Category()
        {
            Name = "Clothing",
            Description = "Apparel for men, women, and children in various styles and sizes",
            SubCategories = new List<SubCategory>()
            {
                new()
                {
                    Name = "Men's Wear",
                    Description = "Stylish and comfortable clothing for men"
                },
                new()
                {
                    Name = "Women's Wear",
                    Description = "Fashionable women's clothing for all occasions"
                },
                new()
                {
                    Name = "Children's Wear",
                    Description = "Durable and cute clothing for children"
                }
            }
        },
        new Category()
        {
            Name = "Home Appliances",
            Description = "Essential appliances for home such as refrigerators, washing machines, and microwaves",
            SubCategories = new List<SubCategory>
            {
                new()
                {
                    Name = "Kitchen Appliances",
                    Description = "Appliances for kitchen use, like microwaves, ovens, and toasters"
                },
                new()
                {
                    Name = "Laundry Appliances",
                    Description = "Appliances for laundry, including washing machines and dryers"
                },
                new()
                {
                    Name = "Small Appliances",
                    Description = "Small household appliances like blenders, coffee makers, and irons"
                }
            }
        },
        new Category()
        {
            Name = "Books",
            Description = "A wide range of books from fiction to educational textbooks",
            SubCategories = new List<SubCategory>
            {
                new()
                {
                    Name = "Fiction",
                    Description = "Novels and stories spanning various genres"
                },
                new()
                {
                    Name = "Non-Fiction",
                    Description = "Books covering real-life subjects and events"
                },
                new()
                {
                    Name = "Textbooks",
                    Description = "Educational textbooks for different subjects and levels"
                },
                new()
                {
                    Name = "Children's Books",
                    Description = "Books for children including picture books and early reading material"
                }
            }
        },
        new Category()
        {
            Name = "Fitness",
            Description = "Fitness equipment and accessories including weights, yoga mats, and treadmills",
            SubCategories = new List<SubCategory>
            {
                new()
                {
                    Name = "Gym Equipment",
                    Description = "Equipment for gym workouts like weights and machines"
                },
                new()
                {
                    Name = "Yoga Accessories",
                    Description = "Accessories for yoga, including mats, blocks, and straps"
                },
                new()
                {
                    Name = "Outdoor Fitness",
                    Description = "Equipment for outdoor fitness activities, such as running gear and bicycles"
                }
            }
        }
    };
}