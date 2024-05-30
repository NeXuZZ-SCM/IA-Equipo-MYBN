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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AIProductController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AIProductController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AIProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AIProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
