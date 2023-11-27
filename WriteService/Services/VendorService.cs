using System.Transactions;
using AutoMapper;
using Common.RabbitMQ;
using Common.RabbitMQ.MessageDTOs;
using Microsoft.EntityFrameworkCore;
using SharpCompress.Common;
using WriteService.DTOs.Vendor;
using WriteService.Entities;
using WriteService.Exceptions;

namespace WriteService.Services;

public class VendorService
{
    private readonly ShopDbContext _context;
    private readonly ILogger<VendorService> _logger;
    private readonly IMapper _mapper;
    private readonly RabbitMQProducer _producer;

    public VendorService(ShopDbContext context, ILogger<VendorService> logger, IMapper mapper, RabbitMQProducer producer)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _producer = producer;
    }

    public async Task<VendorEntity> CreateAsync(CreateVendorDto dto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var vendor = new VendorEntity()
                {
                    Name = dto.Name,
                    Country = dto.Country,
                    ZipCode = dto.ZipCode,
                    City = dto.City,
                    Street = dto.Street,
                    HouseNumber = dto.HouseNumber
                };

                _context.Add(vendor);


                var saveChangesTask = _context.SaveChangesAsync();
                var vendorMessageDto = _mapper.Map<VendorMessageDTO>(vendor);
                var sendMessageTask = _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.Vendor, vendorMessageDto);

                await Task.WhenAll(saveChangesTask, sendMessageTask);

                scope.Complete();
                return vendor;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating vendor: {ex.Message}", ex);
                throw;
            }
            finally
            {
                scope.Dispose();
            }
        }
    }

    public async Task<VendorEntity> UpdateAsync(long vendorId, UpdateVendorDto dto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var vendor = await FindVendorAsync(vendorId);

                vendor.Country = dto.Country;
                vendor.ZipCode = dto.ZipCode;
                vendor.City = dto.City;
                vendor.Street = dto.Street;
                vendor.HouseNumber = dto.HouseNumber;

                _context.Update(vendor);
                var saveChangesTask = _context.SaveChangesAsync();
                var vendorMessageDto = _mapper.Map<VendorMessageDTO>(vendor);
                var sendMessageTask = _producer.SendMessageAsync(RabbitMQOperation.Update, RabbitMQEntities.Vendor, vendorMessageDto);

                await Task.WhenAll(saveChangesTask, sendMessageTask);

                scope.Complete();
                return vendor;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating vendor: {ex.Message}", ex);
                throw;
            }
        }
    }

    public async Task DeleteAsync(long vendorId)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var vendor = await FindVendorAsync(vendorId, includeProducts: true);

                vendor.IsDeleted = true;
                foreach (var product in vendor.Products)
                {
                    product.IsDeleted = true;
                }

                _context.Update(vendor);

                var saveChangesTask = _context.SaveChangesAsync();
                var vendorMessageDto = _mapper.Map<VendorMessageDTO>(vendor);
                var sendMessageTask = _producer.SendMessageAsync(RabbitMQOperation.Delete, RabbitMQEntities.Vendor, vendorMessageDto);

                await Task.WhenAll(saveChangesTask, sendMessageTask);

                scope.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting vendor: {ex.Message}", ex);
                throw;
            }
        }
    }

    private async Task<VendorEntity> FindVendorAsync(long id, bool includeProducts = false)
    {
        var query = _context
            .Vendors
            .AsQueryable();

        if (includeProducts)
        {
            query.Include(x => x.Products);
        }

        var vendor = await query.FirstOrDefaultAsync(x => x.Id == id);
        if (vendor is null)
        {
            throw new EntityNotFoundException(id);
        }

        return vendor;
    }
}