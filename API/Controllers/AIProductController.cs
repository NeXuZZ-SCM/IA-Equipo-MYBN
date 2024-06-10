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

        //[HttpGet("predict/co-products")]
        //public ContentResult Get(int id, int count)
        //{
        //    string jsonResponse = @"[
        //                                {
        //                                    ""id"": 1,
        //                                    ""name"": ""Co-Producto A"",
        //                                    ""score"": 6.99
        //                                },
        //                                {
        //                                    ""id"": 2,
        //                                    ""name"": ""Co-Producto B"",
        //                                    ""score"": 5.1
        //                                },
        //                                {
        //                                    ""id"": 3,
        //                                    ""name"": ""Co-Producto C"",
        //                                    ""score"": 4.8
        //                                }
        //                            ]";
        //    return Content(jsonResponse, "application/json");
        //}

        [HttpGet("predict/co-products/GetRecommendedProducts")]
        public async Task<List<CopurchasePrediction2>> GetRecommendedProducts(int id, int limit)
        {
            if (limit is 0)
            {
                limit = 3;
            }
            IEnumerable<(float CoProductId, float Score)> response = await _productService.GetRecommendedProducts(id, limit);

            List<CopurchasePrediction2> responseList = new List<CopurchasePrediction2>();
            foreach (var responseItem in response)
            {
                CopurchasePrediction2 copurchasePrediction2 = new CopurchasePrediction2();
                copurchasePrediction2.Id = responseItem.CoProductId;
                copurchasePrediction2.Score = responseItem.Score;
                responseList.Add(copurchasePrediction2);
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
