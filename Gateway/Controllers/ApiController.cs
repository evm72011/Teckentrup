using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using Models;
using Gateway.Models;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly HttpDataLoader dataLoader;

        public ApiController(IHttpClientFactory clientFactory)
        {
            this.dataLoader = new HttpDataLoader(clientFactory);
        }

        [HttpGet]
        [Route("data")]
        public IEnumerable<Brand> GetData(string url)
        {
            return dataLoader.Get<IEnumerable<Brand>>(url).Result;
        }

        [HttpGet]
        [Route("all_brand_names")]
        public IEnumerable<string> GetAllBrandNames(string url)
        {
            var data = dataLoader.Get<IEnumerable<Brand>>(url).Result;
            var repo = new BrandsRepository(data);
            return repo.GetAllBrandNames();
        }

        [HttpGet]
        [Route("all_articles")]
        public IEnumerable<Article> GetAllArticles(string url, string brandName)
        {
            var data = dataLoader.Get<IEnumerable<Brand>>(url).Result;
            var repo = new BrandsRepository(data);
            return repo.GetAllArticles();
        }

        [HttpGet]
        [Route("articles_with_max_price")]
        public IEnumerable<Article> GetArticlesWithMaxPrice(string url)
        {
            var data = dataLoader.Get<IEnumerable<Brand>>(url).Result;
            var repo = new BrandsRepository(data);
            return repo.GetArticlesWithMaxPrice();
        }

        [HttpGet]
        [Route("articles_with_min_price")]
        public IEnumerable<Article> GetArticlesWithMinPrice(string url)
        {
            var data = dataLoader.Get<IEnumerable<Brand>>(url).Result;
            var repo = new BrandsRepository(data);
            return repo.GetArticlesWithMinPrice();
        }

        [HttpGet]
        [Route("articles_by_price")]
        public IEnumerable<Article> GetArticlesByPrice(string url, decimal price)
        {
            var data = dataLoader.Get<IEnumerable<Brand>>(url).Result;
            var repo = new BrandsRepository(data);
            return repo.GetArticlesByPrice(price);
        }

        [HttpGet]
        [Route("answer_to_all_questions")]
        public AnswerToAllQuestions GetAnswerToAllQuestions(string url, string brandName, decimal price)
        {
            var data = dataLoader.Get<IEnumerable<Brand>>(url).Result;
            var repo = new BrandsRepository(data);
            var answer = new AnswerToAllQuestions
            {
                AllBrandNames       = repo.GetAllBrandNames(),
                ArticlesByBrandName = repo.GetArticlesByBrandName(brandName),
                ExpensivestArticles = repo.GetArticlesWithMaxPrice(),
                CheapestArticles    = repo.GetArticlesWithMinPrice(),
                ArticlesWithPrice   = repo.GetArticlesByPrice(price)
            };
            return answer;
        }
    }
}