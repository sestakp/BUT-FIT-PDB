using AutoMapper;
using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using Common.RabbitMQ.Messages.Vendor;
using Microsoft.EntityFrameworkCore;
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
        await using (var transaction = await _context.Database.BeginTransactionAsync())
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

            await _context.SaveChangesAsync();

            var message = new CreateVendorMessage()
            {
                Id = vendor.Id,
                Name = vendor.Name,
                AddressCountry = vendor.Country,
                AddressZipCode = vendor.ZipCode,
                AddressCity = vendor.City,
                AddressStreet = vendor.Street,
                AddressHouseNumber = vendor.HouseNumber
            };

            _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.Vendor, message);

            await transaction.CommitAsync();

            return vendor;
        }
    }

    public async Task<VendorEntity> UpdateAsync(long vendorId, UpdateVendorDto dto)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var vendor = await FindVendorAsync(vendorId);

            vendor.Name = dto.Name;
            vendor.Country = dto.Country;
            vendor.ZipCode = dto.ZipCode;
            vendor.City = dto.City;
            vendor.Street = dto.Street;
            vendor.HouseNumber = dto.HouseNumber;

            _context.Update(vendor);

            await _context.SaveChangesAsync();

            var message = new UpdateVendorMessage()
            {
                VendorId = vendor.Id,
                Name = vendor.Name,
                Country = vendor.Country,
                ZipCode = vendor.ZipCode,
                City = vendor.City,
                Street = vendor.Street,
                HouseNumber = vendor.HouseNumber
            };

            _producer.SendMessageAsync(RabbitMQOperation.Update, RabbitMQEntities.Vendor, message);

            await transaction.CommitAsync();

            return vendor;
        }
    }

    public async Task DeleteAsync(long vendorId)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var vendor = await FindVendorAsync(vendorId, includeProducts: true);

            vendor.IsDeleted = true;
            foreach (var product in vendor.Products)
            {
                product.IsDeleted = true;
            }

            _context.Update(vendor);

            await _context.SaveChangesAsync();

            // TODO: send message

            await transaction.CommitAsync();
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