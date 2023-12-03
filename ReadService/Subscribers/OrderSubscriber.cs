using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using MongoDB.Driver;
using RabbitMQ.Client;
using ReadService.Data;
using ReadService.Data.Models;

namespace ReadService.Subscribers;

public class OrderSubscriber : RabbitMQReceiver<OrderSubscriber>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public OrderSubscriber(IModel channel, ILogger<OrderSubscriber> logger, IServiceScopeFactory serviceScopeFactory)
        : base(channel, logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override void HandleCreate(RabbitMQMessage message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            var data = (message.Data as OrderCompletedMessage)!;

            var order = new Order()
            {
                Id = data.Id,
                CustomerId = data.CustomerId,
                Created = data.DateTimeCreated,
                Price = data.Price,
                Address = new OrderAddress()
                {
                    Country = data.AddressCountry,
                    City = data.AddressCity,
                    ZipCode = data.AddressZipCode,
                    Street = data.AddressStreet,
                    HouseNumber = data.AddressHouseNumber
                },
                Products = data.Products.Select(x => new OrderProduct()
                {
                    Title = x.Title,
                    Description = x.Description,
                    Price = x.Price,
                    Count = x.Count,
                    Vendor = new OrderProductVendor() { Name = x.Vendor }
                })
            };

            database.Collection<Order>().InsertOne(order);

            _logger.LogInformation("Inserted one new document to {Collection} collection.", nameof(Product));

            var updateCounter = 0;

            foreach (var product in data.Products)
            {
                var filter = Builders<Product>
                    .Filter
                    .Eq(p => p.Id, product.Id);

                var updateDefinition = Builders<Product>
                    .Update
                    .Inc(p => p.PiecesInStock, -product.Count);

                database.Collection<Product>().UpdateOne(filter, updateDefinition);

                updateCounter++;
            }

            _logger.LogInformation("Updated {Count} document from {Collection} collection.", updateCounter, nameof(Product));
        }
    }
}