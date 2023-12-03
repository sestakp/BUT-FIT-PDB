using WriteService.Entities;

namespace WriteService;

public static class Seeds
{
    public static void ApplyDatabaseSeeds(ShopDbContext dbContext)
    {
        var category = new ProductCategoryEntity
        {
            Name = "Electronics",
            Description = "Devices and gadgets including smartphones, laptops, and cameras",
            NormalizedName = "electronics"
        };

        dbContext.Add(category);

        var subCategory = new ProductSubCategoryEntity()
        {
            Name = "Smartphones",
            Description = "Latest and advanced smartphones",
            NormalizedName = "smartphones",
            Category = category
        };

        dbContext.Add(subCategory);

        var subCategory2 = new ProductSubCategoryEntity()
        {
            Name = "Laptops",
            Description = "High-performance and portable laptops",
            NormalizedName = "laptops",
            Category = category
        };

        dbContext.Add(subCategory2);
        var subCategory3 = new ProductSubCategoryEntity()
        {
            Name = "Cameras",
            Description = "Digital cameras for professional photography",
            NormalizedName = "cameras",
            Category = category
        };

        dbContext.Add(subCategory3);

        category.SubCategories.Add(subCategory);
        category.SubCategories.Add(subCategory2);
        category.SubCategories.Add(subCategory3);





        category = new ProductCategoryEntity
        {
            Name = "Clothing",
            Description = "Apparel for men, women, and children in various styles and sizes",
            NormalizedName = "clothing"
        };

        dbContext.Add(category);

        subCategory = new ProductSubCategoryEntity()
        {
            Name = "Men's Wear",
            Description = "Stylish and comfortable clothing for men",
            NormalizedName = "mens-wear",
            Category = category
        };

        dbContext.Add(subCategory);

        subCategory2 = new ProductSubCategoryEntity()
        {
            Name = "Women's Wear",
            Description = "Fashionable women's clothing for all occasions",
            NormalizedName = "womens-wear",
            Category = category
        };

        dbContext.Add(subCategory2);
        subCategory3 = new ProductSubCategoryEntity()
        {
            Name = "Children's Wear",
            Description = "Durable and cute clothing for children",
            NormalizedName = "childrens-wear",
            Category = category
        };

        dbContext.Add(subCategory3);

        category.SubCategories.Add(subCategory);
        category.SubCategories.Add(subCategory2);
        category.SubCategories.Add(subCategory3);



        category = new ProductCategoryEntity
        {
            Name = "Home Appliances",
            Description = "Essential appliances for home such as refrigerators, washing machines, and microwaves",
            NormalizedName = "home-appliances"
        };

        dbContext.Add(category);

        subCategory = new ProductSubCategoryEntity()
        {
            Name = "Kitchen Appliances",
            Description = "Appliances for kitchen use, like microwaves, ovens, and toasters",
            NormalizedName = "kitchen-appliances",
            Category = category
        };

        dbContext.Add(subCategory);

        subCategory2 = new ProductSubCategoryEntity()
        {
            Name = "Laundry Appliances",
            Description = "Appliances for laundry, including washing machines and dryers",
            NormalizedName = "laundry-appliances",
            Category = category
        };

        dbContext.Add(subCategory2);
        subCategory3 = new ProductSubCategoryEntity()
        {
            Name = "Small Appliances",
            Description = "Small household appliances like blenders, coffee makers, and irons",
            NormalizedName = "small-appliances",
            Category = category
        };

        dbContext.Add(subCategory3);

        category.SubCategories.Add(subCategory);
        category.SubCategories.Add(subCategory2);
        category.SubCategories.Add(subCategory3);



        category = new ProductCategoryEntity
        {
            Name = "Books",
            Description = "A wide range of books from fiction to educational textbooks",
            NormalizedName = "books"
        };

        dbContext.Add(category);

        subCategory = new ProductSubCategoryEntity()
        {
            Name = "Fiction",
            Description = "Novels and stories spanning various genres",
            NormalizedName = "fiction",
            Category = category
        };

        dbContext.Add(subCategory);

        subCategory2 = new ProductSubCategoryEntity()
        {
            Name = "Non-Fiction",
            Description = "Books covering real-life subjects and events",
            NormalizedName = "non-fiction",
            Category = category
        };

        dbContext.Add(subCategory2);
        subCategory3 = new ProductSubCategoryEntity()
        {
            Name = "Textbooks",
            Description = "Educational textbooks for different subjects and levels",
            NormalizedName = "textbooks",
            Category = category
        };

        dbContext.Add(subCategory3);

        var subCategory4 = new ProductSubCategoryEntity()
        {
            Name = "Children's Books",
            Description = "Books for children including picture books and early reading material",
            NormalizedName = "childrens-books",
            Category = category
        };

        dbContext.Add(subCategory4);

        category.SubCategories.Add(subCategory);
        category.SubCategories.Add(subCategory2);
        category.SubCategories.Add(subCategory3);
        category.SubCategories.Add(subCategory4);






        category = new ProductCategoryEntity
        {
            Name = "Fitness",
            Description = "Fitness equipment and accessories including weights, yoga mats, and treadmills",
            NormalizedName = "fitness",
        };

        dbContext.Add(category);

        subCategory = new ProductSubCategoryEntity()
        {
            Name = "Gym Equipment",
            Description = "Equipment for gym workouts like weights and machines",
            NormalizedName = "gym-equipment",
            Category = category
        };

        dbContext.Add(subCategory);

        subCategory2 = new ProductSubCategoryEntity()
        {
            Name = "Yoga Accessories",
            Description = "Accessories for yoga, including mats, blocks, and straps",
            NormalizedName = "yoga-accessories",
            Category = category
        };

        dbContext.Add(subCategory2);
        subCategory3 = new ProductSubCategoryEntity()
        {
            Name = "Outdoor Fitness",
            Description = "Equipment for outdoor fitness activities, such as running gear and bicycles",
            NormalizedName = "outdoor-fitness",
            Category = category
        };

        dbContext.Add(subCategory3);

        category.SubCategories.Add(subCategory);
        category.SubCategories.Add(subCategory2);
        category.SubCategories.Add(subCategory3);
    }
    
}