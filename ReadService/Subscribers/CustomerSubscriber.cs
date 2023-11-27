using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using MongoDB.Driver;
using RabbitMQ.Client;
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

            var collection = database.GetCollection<Customer>(nameof(Customer));

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
}