using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Models;

namespace Gateway.Models
{
    public class HttpBrandRepositoryMock: HttpBrandRepository
    {
        public void LoadDataFromJson(string fileName)
        {
            var jsonString = System.IO.File.ReadAllText(fileName);
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            data = JsonSerializer.Deserialize<IEnumerable<Brand>>(jsonString, serializeOptions);
        }
    }
}
