using API.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Service
{
    public interface IAIProductService
    {
        Task<CopurchasePrediction> GetProductInfo(uint idProduct, uint idCoProduct);
        Task<IEnumerable<(float CoProductId, float Score)>> GetRecommendedProducts(int id, int count);
        Task<ProductCoPurchase?> GetById(int id);
        Task<ProductCoPurchase> Crear(ProductCoPurchase productCoPurchase);
        Task TrainML();
    }
}
