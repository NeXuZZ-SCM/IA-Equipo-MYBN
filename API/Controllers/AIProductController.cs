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
        // GET: api/<AIProductController>
        [HttpGet]
        public CopurchasePrediction Get(uint idProduct, uint idCoProduct)
        {
            return AIProductService.GetProductInfo(idProduct, idCoProduct);
        }

        // GET api/<AIProductController>/5
        [HttpGet("{id}")]
        public ContentResult Get(int id)
        {
            string jsonResponse = @"[
                                        {
                                            ""id"": 1,
                                            ""name"": ""Co-Producto A"",
                                            ""score"": 6.99
                                        },
                                        {
                                            ""id"": 2,
                                            ""name"": ""Co-Producto B"",
                                            ""score"": 5.1
                                        },
                                        {
                                            ""id"": 3,
                                            ""name"": ""Co-Producto C"",
                                            ""score"": 4.8
                                        }
                                    ]";
            return Content(jsonResponse, "application/json");
        }

        #region others

        //// POST api/<AIProductController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<AIProductController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AIProductController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        #endregion
    }
}
