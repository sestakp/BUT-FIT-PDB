using Common.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using SharpCompress.Common;
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

    public async void AnonymizeAsync(long customerId)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
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


                var sendMessageTask =
                    _producer.SendMessageAsync(RabbitMQOperation.Delete, RabbitMQEntities.Customer, customer);
                var saveChangesTask = _context.SaveChangesAsync();

                await Task.WhenAll(saveChangesTask, sendMessageTask);
                scope.Complete();
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                _logger.LogError($"Error anonymizing customer: {ex.Message}", ex);
                throw;
            }
        }
    }

    public async Task<AddressEntity> CreateCustomerAddressAsync(long customerId, CreateAddressDto dto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var customerExists = await _context
                    .Customers
                    .AnyAsync(x => x.Id == customerId);

                if (!customerExists)
                {
                    throw new EntityNotFoundException(customerId);
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
                var sendMessageTask = _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.Address, address);

                var saveChangesTask = _context.SaveChangesAsync();
                await Task.WhenAll(saveChangesTask, sendMessageTask);
                scope.Complete();
                return address;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                _logger.LogError($"Error creating customer address: {ex.Message}", ex);
                throw;
            }
        }
    }

    public async Task<AddressEntity> UpdateCustomerAddressAsync(
        long customerId,
        long addressId,
        UpdateAddressDto dto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
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
                var sendMessageTask = _producer.SendMessageAsync(RabbitMQOperation.Update, RabbitMQEntities.Address, address);

                var saveChangesTask = _context.SaveChangesAsync();
                await Task.WhenAll(saveChangesTask, sendMessageTask);

                scope.Complete();
                return address;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                _logger.LogError($"Error updating customer address: {ex.Message}", ex);
                throw;
            }
        }
    }

    public async void DeleteCustomerAddressAsync(
        long customerId,
        long addressId)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
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

                _context.Remove(address);

                var sendMessageTask =
                    _producer.SendMessageAsync(RabbitMQOperation.Delete, RabbitMQEntities.Address, address);

                var saveChangesTask = _context.SaveChangesAsync();
                await Task.WhenAll(saveChangesTask, sendMessageTask);
                scope.Complete();
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                _logger.LogError($"Error deleting customer address: {ex.Message}", ex);
                throw;
            }
        }
    }
}