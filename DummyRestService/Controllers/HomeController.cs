using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System.Text.Json;

namespace DummyRestService.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Brand> Get()
        {
            var jsonString = System.IO.File.ReadAllText("products.json");
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<IEnumerable<Brand>>(jsonString, serializeOptions);
            return result;
        }
    }
}
