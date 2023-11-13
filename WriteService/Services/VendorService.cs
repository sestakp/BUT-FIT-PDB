using Microsoft.EntityFrameworkCore;
using WriteService.DTOs.Vendor;
using WriteService.Entities;
using WriteService.Exceptions;

namespace WriteService.Services
{
    public class VendorService
    {
        private readonly ShopDbContext _context;

        public VendorService(ShopDbContext context)
        {
            _context = context;
        }

        public VendorEntity Create(CreateVendorDto dto)
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
            _context.SaveChanges();

            return vendor;
        }

        public VendorEntity Update(long vendorId, UpdateVendorDto dto)
        {
            var vendor = FindVendor(vendorId);

            vendor.Country = dto.Country;
            vendor.ZipCode = dto.ZipCode;
            vendor.City = dto.City;
            vendor.Street = dto.Street;
            vendor.HouseNumber = dto.HouseNumber;

            _context.Update(vendor);
            _context.SaveChanges();

            return vendor;
        }

        public void Delete(long vendorId)
        {
            var vendor = FindVendor(vendorId, includeProducts: true);

            vendor.IsDeleted = true;
            foreach (var product in vendor.Products)
            {
                product.IsDeleted = true;
            }

            _context.Update(vendor);
            _context.SaveChanges();
        }

        private VendorEntity FindVendor(long id, bool includeProducts = false)
        {
            var query = _context
                .Vendors
                .AsQueryable();

            if (includeProducts)
            {
                query.Include(x => x.Products);
            }

            var vendor = query.FirstOrDefault(x => x.Id == id);
            if (vendor is null)
            {
                throw new EntityNotFoundException(id);
            }

            return vendor;
        }
    }
}