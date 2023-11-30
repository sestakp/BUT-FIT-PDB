using Common.RabbitMQ;
using Common.RabbitMQ.Messages.Vendor;
using MongoDB.Driver;
using RabbitMQ.Client;
using ReadService.Data;
using ReadService.Data.Models;

namespace ReadService.Subscribers;

public class VendorSubscriber : RabbitMQReceiver<VendorSubscriber>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public VendorSubscriber(IModel channel, ILogger<VendorSubscriber> logger, IServiceScopeFactory serviceScopeFactory)
        : base(channel, logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override void HandleCreate(RabbitMQMessage message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            var data = (message.Data as CreateVendorMessage)!;

            var document = new Vendor()
            {
                Id = data.Id,
                Name = data.Name,
                Address = new VendorAddress()
                {
                    Country = data.AddressCountry,
                    City = data.AddressCity,
                    ZipCode = data.AddressZipCode,
                    Street = data.AddressStreet,
                    HouseNumber = data.AddressHouseNumber
                }
            };

            database.Collection<Vendor>().InsertOne(document);

            _logger.LogInformation("Inserted new document into Vendors collection.");
        }
    }

    protected override void HandleUpdate(RabbitMQMessage message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            var data = (message.Data as UpdateVendorMessage)!;

            var filter = Builders<Vendor>
                .Filter
                .Eq(x => x.Id, data.VendorId);

            var updateDefinition = Builders<Vendor>
                .Update
                .Set(x => x.Name, data.Name)
                .Set(x => x.Address.Country, data.Country)
                .Set(x => x.Address.ZipCode, data.ZipCode)
                .Set(x => x.Address.City, data.City)
                .Set(x => x.Address.Street, data.Street)
                .Set(x => x.Address.HouseNumber, data.HouseNumber);

            var result = database
                .Collection<Vendor>()
                .UpdateOne(filter, updateDefinition);

            _logger.LogInformation("Updated {Count} documents in Vendors collection.", result.MatchedCount);
        }
    }

    protected override void HandleDelete(RabbitMQMessage message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            var data = (message.Data as UpdateVendorMessage)!;
            var vendorId = data.VendorId;

            // Delete vendor from Vendors collection
            {
                var filter = Builders<Vendor>
                    .Filter
                    .Eq(x => x.Id, vendorId);

                var result = database
                    .Collection<Vendor>()
                    .DeleteOne(filter);

                _logger.LogInformation("Deleted {Count} documents from Vendors collection.", result.DeletedCount);
            }


            // Delete all products of the vendor from Products, ProductsOfCategory and ProductsOfSubcategory collections
            {
                IEnumerable<long> removedProducts;

                {
                    var productsFilter = Builders<Product>
                        .Filter
                        .Eq(x => x.Vendor.Id, vendorId);

                    // Get product ids before deletion to be able to remove related reviews
                    removedProducts = database
                        .Collection<Product>()
                        .Find(productsFilter)
                        .ToList()
                        .Select(x => x.Id);

                    var result = database
                        .Collection<Product>()
                        .DeleteMany(productsFilter);

                    _logger.LogInformation("Deleted {Count} documents from Products collection.", result.DeletedCount);
                }
                {
                    var productsOfCategoryFilter = Builders<ProductsOfCategory>
                        .Filter
                        .ElemMatch(x => x.Products, product => product.Vendor.Id == vendorId);

                    var productsOfCategoryUpdateDefinition = Builders<ProductsOfCategory>
                        .Update
                        .PullFilter(x => x.Products, product => product.Vendor.Id == vendorId);

                    var result = database
                        .Collection<ProductsOfCategory>()
                        .UpdateMany(productsOfCategoryFilter, productsOfCategoryUpdateDefinition);

                    _logger.LogInformation("Updated {Count} documents from ProductsOfCategory collection.", result.MatchedCount);
                }
                {
                    var productsOfSubCategoryFilter = Builders<ProductsOfSubCategory>
                        .Filter
                        .ElemMatch(x => x.Products, product => product.Vendor.Id == vendorId);

                    var productsOfSubCategoryUpdateDefinition = Builders<ProductsOfSubCategory>
                        .Update
                        .PullFilter(x => x.Products, product => product.Vendor.Id == vendorId);

                    var result = database
                        .Collection<ProductsOfSubCategory>()
                        .UpdateMany(productsOfSubCategoryFilter, productsOfSubCategoryUpdateDefinition);

                    _logger.LogInformation("Updated {Count} documents from ProductsOfSubCategory collection.", result.MatchedCount);
                }
                {
                    var reviewsFilter = Builders<Review>
                        .Filter
                        .In(x => x.ProductId, removedProducts);

                    var result = database
                        .Collection<Review>()
                        .DeleteMany(reviewsFilter);

                    _logger.LogInformation("Deleted {Count} documents from Reviews collection.", result.DeletedCount);
                }
            }
        }
    }
}