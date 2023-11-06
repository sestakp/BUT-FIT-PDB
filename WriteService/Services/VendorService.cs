using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WriteService.DTO;
using WriteService.Entities;

namespace WriteService.Services
{
    public class VendorService
    {

        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;
        public VendorService(ShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        

        public VendorEntity CreateOrUpdate(VendorDto vendorDto)
        {
            using var scope = new TransactionScope();
            // Add your create logic here
            

            var storedEntity = _context.Vendors.Find(vendorDto.Id);

            var vendorEntity = _mapper.Map<VendorEntity>(vendorDto);

            if (storedEntity != null)
            {
                _context.Vendors.Update(vendorEntity);
            }
            else
            {
                _context.Vendors.Add(vendorEntity);
            }
            _context.SaveChanges();
            
            scope.Complete();

            return vendorEntity;
        }

        public bool SoftDelete(long vendorId)
        {
            using var scope = new TransactionScope();
            var vendor = _context.Vendors
                .Include(v => v.Products) // Include the related products
                .FirstOrDefault(v => v.Id == vendorId);

            if (vendor != null)
            {
                vendor.isDeleted = true;

                foreach (var product in vendor.Products)
                {
                    product.isDeleted = true;
                }

                _context.SaveChanges();
            }

            scope.Complete();

            return vendor != null;
        }
    }
}
