using API.Model;

namespace API.Service
{
    public interface IAIProductService
    {
        Task<CopurchasePrediction> GetProductInfo(uint idProduct, uint idCoProduct);
        Task<ProductCoPurchase?> GetById(int id);
        Task<ProductCoPurchase> Crear(ProductCoPurchase productCoPurchase);
        Task TrainML();
    }
}
