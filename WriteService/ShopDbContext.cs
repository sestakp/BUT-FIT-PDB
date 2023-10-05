using Microsoft.EntityFrameworkCore;
using WriteService.Entities;

namespace WriteService
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; } = null!;
    }
}
