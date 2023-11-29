using Common.RabbitMQ;
using Common.RabbitMQ.Messages.Customer;
using MongoDB.Bson;
using MongoDB.Driver;
using RabbitMQ.Client;
using ReadService.Data;
using ReadService.Data.Models;

namespace ReadService.Subscribers;

public class CustomerSubscriber : RabbitMQReceiver<CustomerSubscriber>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CustomerSubscriber(IModel channel, ILogger<CustomerSubscriber> logger, IServiceScopeFactory serviceScopeFactory)
        : base(channel, logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override void HandleCreate(RabbitMQMessage message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            var data = (message.Data as CreateCustomerMessage)!;

            var collection = database.Collection<Customer>();

            var document = new Customer()
            {
                Email = data.Email,
                FirstName = data.FirstName,
                LastName = data.LastName,
                PhoneNumber = data.PhoneNumber
            };

            collection.InsertOne(document);
        }
    }

    protected override void HandleUpdate(RabbitMQMessage message)
    {
        switch (message.Data)
        {
            case (UpdateCustomerMessage m):
                UpdateCustomer(m);
                break;
            case (AddCustomerAddressMessage m):
                AddCustomerAddress(m);
                break;
            case (UpdateCustomerAddressMessage m):
                UpdateCustomerAddress(m);
                break;
            case (DeleteCustomerAddressMessage m):
                DeleteCustomerAddress(m);
                break;
            default:
                throw new Exception("Unexpected message type.");
        }

        return;

        void UpdateCustomer(UpdateCustomerMessage data)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

                var filter = Builders<Customer>
                    .Filter
                    .Eq(x => x.Email, data.CustomerEmail);

                var updateDefinition = Builders<Customer>
                    .Update
                    .Set(x => x.FirstName, data.FirstName)
                    .Set(x => x.LastName, data.LastName);

                var result = database
                    .Collection<Customer>()
                    .UpdateOne(filter, updateDefinition);

                _logger.LogInformation("Updated {Count} documents in Customers collection.", result.MatchedCount);
            }
        }

        void AddCustomerAddress(AddCustomerAddressMessage data)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

                var address = new Address()
                {
                    Id = data.Id,
                    Country = data.Country,
                    ZipCode = data.ZipCode,
                    City = data.City,
                    HouseNumber = data.HouseNumber,
                    Street = data.Street
                };

                var filter = Builders<Customer>
                    .Filter
                    .Eq(x => x.Email, data.CustomerEmail);

                var updateDefinition = Builders<Customer>
                    .Update
                    .AddToSet(x => x.Addresses, address);

                var result = database
                    .Collection<Customer>()
                    .UpdateOne(filter, updateDefinition);

                _logger.LogInformation("Updated {Count} documents in Customers collection.", result.MatchedCount);
            }
        }

        void UpdateCustomerAddress(UpdateCustomerAddressMessage data)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

                var customerFilter = Builders<Customer>
                    .Filter
                    .Eq(x => x.Email, data.CustomerEmail);

                var addressFilter = Builders<Customer>
                    .Filter
                    .ElemMatch(x => x.Addresses, a => a.Id == data.AddressId);

                var updateDefinition = Builders<Customer>
                    .Update
                    .Set("addresses.$.country", data.Country)
                    .Set("addresses.$.zipCode", data.ZipCode)
                    .Set("addresses.$.city", data.City)
                    .Set("addresses.$.street", data.Street)
                    .Set("addresses.$.houseNumber", data.HouseNumber);

                var result = database
                    .Collection<Customer>()
                    .UpdateOne(customerFilter & addressFilter, updateDefinition);

                _logger.LogInformation("Updated {Count} documents in Customers collection.", result.MatchedCount);
            }
        }

        void DeleteCustomerAddress(DeleteCustomerAddressMessage data)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

                var customerFilter = Builders<Customer>
                    .Filter
                    .Eq(x => x.Email, data.CustomerEmail);

                var updateDefinition = Builders<Customer>
                    .Update
                    .PullFilter(x => x.Addresses, x => x.Id == data.AddressId);

                var result = database
                    .Collection<Customer>()
                    .UpdateOne(customerFilter, updateDefinition);

                _logger.LogInformation("Removed {Count} customer addresses.", result.MatchedCount);
            }
        }
    }

    protected override void HandleDelete(RabbitMQMessage message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            var data = (message.Data as DeleteCustomerMessage)!;

            var customerFilter = Builders<Customer>
                .Filter
                .Eq(x => x.Email, data.CustomerEmail);

            var r1 = database.Collection<Customer>().DeleteOne(customerFilter);

            _logger.LogInformation("Removed {Count} customers.", r1.DeletedCount);

            var ordersFilter = Builders<Order>
                .Filter
                .Eq(x => x.CustomerEmail, data.CustomerEmail);

            var r2 = database.Collection<Order>().DeleteMany(ordersFilter);

            _logger.LogInformation("Removed {Count} customer orders.", r2.DeletedCount);
        }
    }
}