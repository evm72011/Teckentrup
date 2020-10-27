using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using System.Net.Http;
using System.Text.Json;

namespace Gateway.Models
{
    /// <summary>
    /// An Http implementation of IBrandRepository
    /// </summary>
    public class HttpBrandRepository : IBrandRepository
    {
        protected IEnumerable<Brand> data = new List<Brand>();

        public OperationResult LoadData(string url)
        {
            var result = new OperationResult();
            if (string.IsNullOrWhiteSpace(url))
            {
                result.Success = false;
                result.Message = "Empty url";
                return result;
            }
            var loadResult = LoadFromHttpAsync<IEnumerable<Brand>>(url).Result;
            data = loadResult.Data;
            data = (data is null) ? new List<Brand>() : data;

            result.Success = loadResult.Success;
            result.Message = loadResult.Message;
            return result;
        }

        private async Task<LoadDataResult<T>> LoadFromHttpAsync<T>(string url)
        {
            var http = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await http.GetAsync(url);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new LoadDataResult<T>
                {
                    Message = "Unexpected exception see log for details"
                };
            }

            var result = new LoadDataResult<T>
            {
                Success = response.IsSuccessStatusCode,
                Message = response.ReasonPhrase
            };

            if (response.IsSuccessStatusCode)
            {
                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                using var responseStream = await response.Content.ReadAsStreamAsync();
                result.Data = await JsonSerializer.DeserializeAsync<T>(responseStream, serializeOptions);
            }
            return result;
        }

        private IEnumerable<Article> GetAllArticles()
        {
            return data.SelectMany(brand => brand.Types).SelectMany(typ => typ.Articles);
        }

        public IEnumerable<string> GetAllBrandNames()
        {
            return data.Select(brand => brand.BrandName);
        }

        public IEnumerable<Article> GetArticlesByBrandName(string brandName)
        {
            IEnumerable<Brand> neededBrands;
            if (string.IsNullOrEmpty(brandName))
            {
                neededBrands = data.Where(data => string.IsNullOrEmpty(data.BrandName));
            }
            else
            {
                neededBrands = data.Where(data => data.BrandName.ToUpper() == brandName.ToUpper());
            }
            return neededBrands.SelectMany(brand => brand.Types).SelectMany(typ => typ.Articles);
        }

        public IEnumerable<Article> GetArticlesWithMaxPrice()
        {
            var articles = GetAllArticles();
            var maxPrice = articles.Select(article => article.PricePerLiter).Max();
            return articles.Where(article => article.PricePerLiter == maxPrice);
        }

        public IEnumerable<Article> GetArticlesWithMinPrice()
        {
            var articles = GetAllArticles();
            var minPrice = articles.Select(article => article.PricePerLiter).Min();
            return articles.Where(article => article.PricePerLiter == minPrice);
        }

        public IEnumerable<Article> GetArticlesByPrice(decimal price)
        {
            var articles = GetAllArticles();
            return articles.Where(article => article.Price == price);
        }
    }
}
