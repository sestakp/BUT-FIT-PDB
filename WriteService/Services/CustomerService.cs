using AutoMapper;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using WriteService.DTO;
using WriteService.Entities;

namespace WriteService.Services
{
    public class CustomerService
    {

        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;
        public CustomerService(ShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public CustomerEntity CreateOrUpdate(CustomerDto customerDto)
        {
            using var scope = new TransactionScope();
            // Add your create logic here


            var storedEntity = _context.Vendors.Find(customerDto.Id);

            var customerEntity = _mapper.Map<CustomerEntity>(customerDto);

            if (storedEntity != null)
            {
                _context.Customers.Update(customerEntity);
            }
            else
            {
                _context.Customers.Add(customerEntity);
            }
            _context.SaveChanges();

            scope.Complete();

            return customerEntity;
        }


        public CustomerEntity Anonymize(long id)
        {
            using var scope = new TransactionScope();

            var customer = _context.Customers.Include(c => c.Addresses).FirstOrDefault(c => c.Id == id);

            if (customer == default)
            {
                return new CustomerEntity();
            }

            customer.Email = "anonymized";
            customer.FirstName = "anonymized";
            customer.LastName = "anonymized";
            customer.PhoneNumber = "anonymized";
            customer.PasswordHash = "anonymized";
            customer.isDeleted = true;

            foreach (var address in customer.Addresses)
            {
                _context.Remove(address);
            }

            _context.SaveChanges();
            scope.Complete();
            return customer;
        }
    }
}
