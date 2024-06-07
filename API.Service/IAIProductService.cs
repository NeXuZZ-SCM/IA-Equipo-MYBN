using API.Model;

namespace API.Service
{
    public interface IAIProductService
    {
        CopurchasePrediction GetProductInfo(uint idProduct, uint idCoProduct);

        Task<ProductCoPurchase?> GetById(int id);
        Task<ProductCoPurchase> Crear(ProductCoPurchase productCoPurchase);
    }
}
