using Common.RabbitMQ;
using Common.RabbitMQ.Messages.Products;
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

            // Add new product to collection of Products
            {
                var collection = database.Collection<Product>();

                var product = new Product()
                {
                    Id = data.Id,
                    Title = data.Title,
                    Description = data.Description,
                    PiecesInStock = data.PiecesInStock,
                    Price = data.Price,
                    Rating = null,
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

                collection.InsertOne(product);

                _logger.LogInformation("Inserted new document into Products collection.");
            }

            // Update collection of ProductsByCategory - add product to collection of products in given category
            {
                var collection = database.Collection<ProductsOfCategory>();

                var product = new ProductsOfCategory.Product()
                {
                    Id = data.Id,
                    Title = data.Title,
                    Description = data.Description,
                    Price = data.Price,
                    Rating = null,
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

                var result = collection.UpdateMany(filter, update);

                _logger.LogInformation("Updated {Count} records in ProductsOfCategory collection.", result.MatchedCount);
            }

            // Update collection of ProductsBySubCategory - add product to collection of products in given category
            {
                var collection = database.Collection<ProductsOfSubCategory>();

                var product = new ProductsOfSubCategory.Product()
                {
                    Id = data.Id,
                    Title = data.Title,
                    Description = data.Description,
                    Price = data.Price,
                    Rating = null,
                    Vendor = new ProductsOfSubCategory.Product.ProductVendor()
                    {
                        Id = data.VendorId,
                        Name = data.VendorName
                    }
                };

                var filter = Builders<ProductsOfSubCategory>
                    .Filter
                    .In(x => x.SubCategoryName, productSubcategories);

                var update = Builders<ProductsOfSubCategory>
                    .Update
                    .Push(x => x.Products, product);

                var result = collection.UpdateMany(filter, update);

                _logger.LogInformation("Updated {Count} records in ProductsOfSubCategory collection.", result.MatchedCount);
            }

            // Update collection of Vendors - try to add category of the new product to vendor
            {
                var collection = database.Collection<Vendor>();

                var filter = Builders<Vendor>
                    .Filter
                    .Eq(x => x.Id, data.VendorId);

                var update = Builders<Vendor>
                    .Update
                    .AddToSetEach(vendor => vendor.Categories, productCategories);

                var result = collection.UpdateOne(filter, update);

                _logger.LogInformation("Updated {Count} records in Vendors collection.", result.MatchedCount);
            }
        }
    }

    protected override void HandleDelete(RabbitMQMessage message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            var data = (message.Data as DeleteProductMessage)!;

            var productId = data.ProductId;

            // Delete product from Product collection
            {
                var filter = Builders<Product>
                    .Filter
                    .Eq(x => x.Id, productId);

                database.Collection<Product>().DeleteOne(filter);

                _logger.LogInformation("Deleted one document from {Collection} collection.", nameof(Product));
            }

            // Delete product from ProductsOfCategory collection
            {
                var filter = Builders<ProductsOfCategory>
                    .Filter
                    .ElemMatch(x => x.Products, product => product.Id == productId);

                var update = Builders<ProductsOfCategory>
                    .Update
                    .PullFilter(x => x.Products, x => x.Id == productId);

                var result = database
                    .Collection<ProductsOfCategory>()
                    .UpdateMany(filter, update);

                _logger.LogInformation("Updated {Count} documents from {Collection} collection.",
                    result.MatchedCount,
                    nameof(ProductsOfCategory));
            }

            // Delete product from ProductsOfSubcategory collection
            {
                var filter = Builders<ProductsOfSubCategory>
                    .Filter
                    .ElemMatch(x => x.Products, product => product.Id == productId);

                var update = Builders<ProductsOfSubCategory>
                    .Update
                    .PullFilter(x => x.Products, x => x.Id == productId);

                var result = database
                    .Collection<ProductsOfSubCategory>()
                    .UpdateMany(filter, update);

                _logger.LogInformation("Updated {Count} documents from {Collection} collection.",
                    result.MatchedCount,
                    nameof(ProductsOfSubCategory));
            }

            // Delete product reviews
            {
                var filter = Builders<Review>
                    .Filter
                    .Eq(x => x.ProductId, productId);

                var result = database
                    .Collection<Review>()
                    .DeleteMany(filter);

                _logger.LogInformation("Deleted {Count} documents from {Collection} collection.",
                    result.DeletedCount,
                    nameof(Product));
            }
        }
    }
}