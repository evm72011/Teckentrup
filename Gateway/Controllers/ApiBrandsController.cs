using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using Models;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class ApiBrandsController : ControllerBase
    {
        private readonly HttpDataLoader dataLoader;
        public ApiBrandsController(IHttpClientFactory clientFactory)
        {
            this.dataLoader = new HttpDataLoader(clientFactory);
        }

        [HttpGet]
        public IEnumerable<Brand> Get(string url)
        {
            return dataLoader.Get<IEnumerable<Brand>>(url).Result;
        }
    }
}