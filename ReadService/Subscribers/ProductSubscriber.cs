using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using MongoDB.Driver;
using RabbitMQ.Client;
using ReadService.Data;
using ReadService.Data.Models;

namespace ReadService.Subscribers;

public class ProductSubscriber : RabbitMQReceiver<ProductSubscriber>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ProductSubscriber(IModel channel, ILogger<ProductSubscriber> logger, IServiceScopeFactory serviceScopeFactory)
        : base(channel, logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override void HandleCreate(RabbitMQMessage message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            var data = (message.Data as CreateProductMessage)!;

            var productCategories = data.Categories.ToList();
            var productSubcategories = data.SubCategories.ToList();

            // Update collection of Products
            {
                var collection = database.Collection<Product>();

                var product = new Product()
                {
                    Id = data.Id,
                    Title = data.Title,
                    Description = data.Description,
                    PiecesInStock = data.PiecesInStock,
                    Price = data.Price,
                    Rating = data.Rating,
                    Vendor = new ProductVendor()
                    {
                        Id = data.VendorId,
                        Name = data.VendorName
                    },
                    Categories = productCategories
                        .Select(x => new ProductCategory() { Name = x })
                        .ToList(),
                    Subcategories = productSubcategories
                        .Select(x => new ProductSubcategory() { Name = x })
                        .ToList(),
                };

                _logger.LogInformation("Inserting record into Products collection.");

                collection.InsertOne(product);
            }

            // Update collection of ProductsByCategory
            {
                var collection = database.Collection<ProductsOfCategory>();

                var product = new ProductsOfCategory.Product()
                {
                    Id = data.Id,
                    Title = data.Title,
                    Description = data.Description,
                    Price = data.Price,
                    Rating = data.Rating,
                    Vendor = new ProductsOfCategory.Product.ProductVendor()
                    {
                        Id = data.VendorId,
                        Name = data.VendorName
                    }
                };

                var filter = Builders<ProductsOfCategory>
                    .Filter
                    .In(x => x.CategoryName, productCategories);

                var update = Builders<ProductsOfCategory>
                    .Update
                    .Push(x => x.Products, product);

                _logger.LogInformation("Updating records in ProductsOfCategory collection.");

                collection.UpdateMany(filter, update);
            }

            // Update collection of ProductsBySubCategory
            {
                var collection = database.Collection<ProductsOfSubCategory>();

                var product = new ProductsOfSubCategory.Product()
                {
                    Id = data.Id,
                    Title = data.Title,
                    Description = data.Description,
                    Price = data.Price,
                    Rating = data.Rating,
                    Vendor = new ProductsOfSubCategory.Product.ProductVendor()
                    {
                        Id = data.VendorId,
                        Name = data.VendorName
                    }
                };

                var filter = Builders<ProductsOfSubCategory>
                    .Filter
                    .In(x => x.Id.SubCategory, productSubcategories);

                var update = Builders<ProductsOfSubCategory>
                    .Update
                    .Push(x => x.Products, product);

                _logger.LogInformation("Updating records in ProductsOfSubCategory collection.");

                collection.UpdateMany(filter, update);
            }

            // Update collection of Vendors.
            {
                var collection = database.Collection<Vendor>();

                var filter = Builders<Vendor>
                    .Filter
                    .Eq(x => x.Id, data.VendorId);

                var update = Builders<Vendor>
                    .Update
                    .AddToSetEach(vendor => vendor.Categories, productCategories);

                _logger.LogInformation("Updating record in Vendors collection.");

                collection.UpdateOne(filter, update);
            }
        }
    }
}