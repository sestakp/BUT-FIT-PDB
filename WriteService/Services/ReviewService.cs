using AutoMapper;
using System.Transactions;
using WriteService.DTO;
using WriteService.Entities;

namespace WriteService.Services
{
    public class ReviewService
    {
        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;
        public ReviewService(ShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ReviewEntity Create(ReviewDto reviewDto)
        {
            using var scope = new TransactionScope();
            // Add your create logic here

            
            var reviewEntity = _mapper.Map<ReviewEntity>(reviewDto);
           
            _context.ProductReviews.Add(reviewEntity);
            
            _context.SaveChanges();

            scope.Complete();
            return reviewEntity;
        }
    }
}
