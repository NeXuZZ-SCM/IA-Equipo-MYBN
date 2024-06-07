using API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace API.Repository
{
    public class AIProductRepository
    {
        private readonly AIProductDbContext _context;

        public AIProductRepository()
        {
            var configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false)).Build();

            var builder = new DbContextOptionsBuilder<AIProductDbContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"), opt =>
                {
                    opt.EnableRetryOnFailure(
                        maxRetryCount: 2,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);
                });

            _context = new AIProductDbContext(builder.Options);
        }

        public async Task<ProductCoPurchase> Insert(ProductCoPurchase productCoPurchase)
        {
            var result = await _context.ProductCoPurchases.AddAsync(productCoPurchase);

            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<ProductCoPurchase?> Get(int id)
        {
            return await _context.ProductCoPurchases.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ProductCoPurchase>> All()
        {
            return await _context.ProductCoPurchases.ToListAsync();
        }

        public async Task<IDataView> GetProductInfo(MLContext mlContext, string trainingDataLocation)
        {
            List<ProductEntry> products = await GetDataProduct();
            return mlContext.Data.LoadFromEnumerable(products);
        }

        public async Task<List<ProductEntry>> GetDataProduct()
        {
            return await _context.ProductCoPurchases
                                .Select(p => new ProductEntry
                                {
                                    Label = 1.0f,
                                    ProductID = Convert.ToUInt32(p.ProductId),
                                    CoPurchaseProductID = Convert.ToUInt32(p.CoPurchaseProductId)
                                }).ToListAsync();
        }
    }
}
