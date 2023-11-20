using Common.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
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
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Suppress))
        {
            try
            {
                var entity = new CustomerEntity()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    PasswordHash = dto.PasswordHash
                };

                _context.Add(entity);

                var saveChangesTask = _context.SaveChangesAsync();

                var sendMessageTask = _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.Customer, entity);

                await Task.WhenAll(saveChangesTask, sendMessageTask);

                scope.Complete();

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating customer: {ex.Message}", ex);
                throw;
            }
        }
    }

    public async Task<CustomerEntity> UpdateAsync(long customerId, UpdateCustomerDto dto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
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

                var saveChangesTask = _context.SaveChangesAsync();

                var sendMessageTask = _producer.SendMessageAsync(RabbitMQOperation.Update, RabbitMQEntities.Customer, entity);

                await Task.WhenAll(saveChangesTask, sendMessageTask);

                // If everything is successful, complete the transaction
                scope.Complete();

                return entity;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                _logger.LogError($"Error updating customer: {ex.Message}", ex);
                throw;
            }
        }
    }

    public void Anonymize(long customerId)
    {
        var customer = _context
            .Customers
            .Include(c => c.Addresses)
            .FirstOrDefault(c => c.Id == customerId);

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

        _context.SaveChanges();
    }

    public AddressEntity CreateCustomerAddress(long customerId, CreateAddressDto dto)
    {
        var customerExists = _context
            .Customers
            .Any(x => x.Id == customerId);

        if (!customerExists)
        {
            throw new Exception($"Cannot create new address because customer with id '{customerId}' does not exist.");
        }

        var address = new AddressEntity()
        {
            Country = dto.Country,
            ZipCode = dto.ZipCode,
            City = dto.City,
            Street = dto.Street,
            HouseNumber = dto.HouseNumber
        };

        _context.Add(address);
        _context.SaveChanges();

        return address;
    }

    public AddressEntity UpdateCustomerAddress(
        long customerId,
        long addressId,
        UpdateAddressDto dto)
    {
        var customer = _context
            .Customers
            .Include(x => x.Addresses)
            .FirstOrDefault(x => x.Id == customerId);

        if (customer is null)
        {
            throw new Exception($"Cannot update address because customer with id '{customerId}' does not exist.");
        }

        var address = customer.Addresses.FirstOrDefault(x => x.Id == addressId);
        if (address is null)
        {
            throw new Exception($"Address with id '{addressId}' does not exist on customer with id '{customerId}'.");
        }

        address.Country = dto.Country;
        address.ZipCode = dto.ZipCode;
        address.City = dto.City;
        address.Street = dto.Street;
        address.HouseNumber = dto.HouseNumber;

        _context.Update(address);
        _context.SaveChanges();

        return address;
    }

    public void DeleteCustomerAddress(
        long customerId,
        long addressId)
    {
        var customer = _context
            .Customers
            .Include(x => x.Addresses)
            .FirstOrDefault(x => x.Id == customerId);

        if (customer is null)
        {
            throw new Exception($"Cannot update address because customer with id '{customerId}' does not exist.");
        }

        var address = customer.Addresses.FirstOrDefault(x => x.Id == addressId);
        if (address is null)
        {
            throw new Exception($"Address with id '{addressId}' does not exist on customer with id '{customerId}'.");
        }

        _context.Remove(address);
        _context.SaveChanges();
    }
}