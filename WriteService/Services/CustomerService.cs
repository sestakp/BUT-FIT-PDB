using Common.RabbitMQ;
using Common.RabbitMQ.Messages.Customer;
using Microsoft.EntityFrameworkCore;
using WriteService.DTOs.Address;
using WriteService.DTOs.Customer;
using WriteService.Entities;
using WriteService.Exceptions;

namespace WriteService.Services;

public class CustomerService
{
    private readonly ShopDbContext _context;
    private readonly RabbitMQProducer _producer;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(ShopDbContext context, RabbitMQProducer producer, ILogger<CustomerService> logger)
    {
        _context = context;
        _producer = producer;
        _logger = logger;
    }

    public async Task<CustomerEntity> CreateAsync(CreateCustomerDto dto)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var entity = new CustomerEntity()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = dto.Password
            };

            _context.Add(entity);

            await _context.SaveChangesAsync();

            var message = new CreateCustomerMessage()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber
            };

            _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.Customer, message);

            await transaction.CommitAsync();

            return entity;
        }
    }

    public async Task<CustomerEntity> UpdateAsync(long customerId, UpdateCustomerDto dto)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var entity = await _context.Customers.FindAsync(customerId);
            if (entity is null)
            {
                throw new EntityNotFoundException(customerId);
            }

            // We do not allow updating email and phone number
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;

            _context.Update(entity);

            await _context.SaveChangesAsync();

            var message = new UpdateCustomerMessage()
            {
                Id = entity.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            _producer.SendMessageAsync(RabbitMQOperation.Update, RabbitMQEntities.Customer, message);

            await transaction.CommitAsync();

            return entity;
        }
    }

    public async Task AnonymizeAsync(long customerId)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var customer = await _context
                .Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer is null)
            {
                throw new EntityNotFoundException(customerId);
            }
            

            const string anonymizationValue = "anonymized";

            customer.Email = anonymizationValue;
            customer.FirstName = anonymizationValue;
            customer.LastName = anonymizationValue;
            customer.PhoneNumber = anonymizationValue;
            customer.PasswordHash = anonymizationValue;
            customer.IsDeleted = true;

            _context.Update(customer);
            foreach (var address in customer.Addresses)
            {
                _context.Remove(address);
            }

            await _context.SaveChangesAsync();

            var message = new DeleteCustomerMessage() { Id = customer.Id };

            _producer.SendMessageAsync(RabbitMQOperation.Delete, RabbitMQEntities.Customer, message);

            await transaction.CommitAsync();
        }
    }

    public async Task<AddressEntity> CreateCustomerAddressAsync(long customerId, CreateAddressDto dto)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var customer = await _context
                .Customers
                .FirstOrDefaultAsync(x => x.Id == customerId);

            if (customer is null)
            {
                throw new EntityNotFoundException(customerId);
            }

            var address = new AddressEntity()
            {
                Country = dto.Country,
                ZipCode = dto.ZipCode,
                City = dto.City,
                Street = dto.Street,
                HouseNumber = dto.HouseNumber,
                CustomerId = customerId
            };


            _context.Add(address);
            customer.Addresses.Add(address);
            _context.Update(customer);

            await _context.SaveChangesAsync();

            var message = new AddCustomerAddressMessage()
            {
                CustomerId = customer.Id,
                Id = address.Id,
                Country = address.Country,
                ZipCode = address.ZipCode,
                City = address.City,
                Street = address.Street,
                HouseNumber = address.HouseNumber
            };

            _producer.SendMessageAsync(RabbitMQOperation.Update, RabbitMQEntities.Customer, message);

            await transaction.CommitAsync();

            return address;
        }
    }

    public async Task<AddressEntity> UpdateCustomerAddressAsync(
        long customerId,
        long addressId,
        UpdateAddressDto dto)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var customer = await _context
                .Customers
                .Include(x => x.Addresses)
                .FirstOrDefaultAsync(x => x.Id == customerId);

            if (customer is null)
            {
                throw new EntityNotFoundException(customerId);
            }

            var address = customer.Addresses.FirstOrDefault(x => x.Id == addressId);
            if (address is null)
            {
                throw new EntityNotFoundException(addressId);
            }

            address.Country = dto.Country;
            address.ZipCode = dto.ZipCode;
            address.City = dto.City;
            address.Street = dto.Street;
            address.HouseNumber = dto.HouseNumber;
            

            _context.Update(address);

            await _context.SaveChangesAsync();

            var message = new UpdateCustomerAddressMessage()
            {
                Id = customer.Id,
                AddressId = address.Id,
                Country = address.Country,
                ZipCode = address.ZipCode,
                City = address.City,
                Street = address.Street,
                HouseNumber = address.HouseNumber
            };

            _producer.SendMessageAsync(RabbitMQOperation.Update, RabbitMQEntities.Customer, message);

            await transaction.CommitAsync();

            return address;
        }
    }

    public async Task DeleteCustomerAddressAsync(
        long customerId,
        long addressId)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var customer = await _context
                .Customers
                .Include(x => x.Addresses)
                .FirstOrDefaultAsync(x => x.Id == customerId);

            if (customer is null)
            {
                throw new EntityNotFoundException(customerId);
            }

            var address = customer.Addresses.FirstOrDefault(x => x.Id == addressId);
            if (address is null)
            {
                throw new EntityNotFoundException(addressId);
            }

            customer.Addresses.Remove(address);
            _context.Remove(address);

            await _context.SaveChangesAsync();

            var message = new DeleteCustomerAddressMessage()
            {
                Id = customer.Id,
                AddressId = addressId
            };

            _producer.SendMessageAsync(RabbitMQOperation.Update, RabbitMQEntities.Customer, message);

            await transaction.CommitAsync();
        }
    }
}