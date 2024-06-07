using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class AIProductDbContext : DbContext
    {
        public AIProductDbContext(DbContextOptions<AIProductDbContext> options) : base(options)
        {
        }

        public DbSet<ProductCoPurchase> ProductCoPurchases { get; set; }
    }
}
