using API.Model;
using API.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIProductController : ControllerBase
    {
        private readonly IAIProductService _productService;

        public AIProductController(IAIProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("predict/co-product-form/")]
        public async Task<CopurchasePrediction> Get(uint idProduct, uint idCoProduct)
        {
            return await _productService.GetProductInfo(idProduct, idCoProduct);
        }


        [HttpGet("predict/co-products/GetRecommendedProducts")]
        public async Task<List<CopurchasePredictionRecommended>> GetRecommendedProducts(int id, int limit)
        {
            if (limit is 0)
                limit = 1;

            IEnumerable<(float CoProductId, float Score)> response = await _productService.GetRecommendedProducts(id, limit);

            List<CopurchasePredictionRecommended> responseList = new List<CopurchasePredictionRecommended>();
            foreach (var responseItem in response)
            {
                CopurchasePredictionRecommended CopurchasePredictionRecommended = new CopurchasePredictionRecommended();
                CopurchasePredictionRecommended.Id = responseItem.CoProductId;
                CopurchasePredictionRecommended.Score = responseItem.Score;
                responseList.Add(CopurchasePredictionRecommended);
            }

            return responseList;
        }


        [HttpGet]
        public async Task<ProductCoPurchase?> GetById(int id)
        {
            return await _productService.GetById(id);
        }

        [HttpPost]
        public async Task<ProductCoPurchase> Crear(ProductCoPurchase productCoPurchase)
        {
            return await _productService.Crear(productCoPurchase);
        }

        [HttpGet("TrainML")]
        public async Task TrainML()
        {
            await _productService.TrainML();
        }
    }
}
