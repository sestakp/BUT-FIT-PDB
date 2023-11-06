using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WriteService.DTO;
using WriteService.Entities;

namespace WriteService.Services
{
    public class AddressService
    {

        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;
        public AddressService(ShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public AddressEntity CreateOrUpdate(AddressDto addressDto)
        {
            using var scope = new TransactionScope();
            // Add your create logic here


            var customer = _context.Customers.Find(addressDto.CustomerId);

            if (customer == null)
            {
                return new AddressEntity();
            }


            var storedEntity = _context.Addresses.Find(addressDto.Id);

            var addressEntity = _mapper.Map<AddressEntity>(addressDto);

            if (storedEntity != null)
            {
                _context.Addresses.Update(addressEntity);
            }
            else
            {
                _context.Addresses.Add(addressEntity);
            }
            _context.SaveChanges();

            scope.Complete();

            return addressEntity;
        }

        public bool SoftDelete(long addressId)
        {
            using var scope = new TransactionScope();
            var address = _context.Addresses.Find(addressId);

            if (address != null)
            {
                _context.Remove(address);
                

                _context.SaveChanges();
            }

            scope.Complete();

            return address != null;
        }
    }
}
