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
using System.Threading;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly IBrandRepository repository;

        public ApiController()
        {
            this.repository = new HttpBrandRepository();
        }

        [HttpGet]
        [Route("all_brand_names")]
        public IEnumerable<string> GetAllBrandNames(string url)
        {
            var result = repository.LoadData(url);
            Response.WrapHeaderWithResult(result);
            return repository.GetAllBrandNames();
        }

        [HttpGet]
        [Route("articles_by_brand_name")]
        public IEnumerable<Article> GetArticlesByBrandName(string url, string brandName)
        {
            var result = repository.LoadData(url);
            Response.WrapHeaderWithResult(result);
            return repository.GetArticlesByBrandName(brandName);
        }

        [HttpGet]
        [Route("articles_with_max_price")]
        public IEnumerable<Article> GetArticlesWithMaxPrice(string url)
        {
            var result = repository.LoadData(url);
            Response.WrapHeaderWithResult(result);
            return repository.GetArticlesWithMaxPrice();
        }

        [HttpGet]
        [Route("articles_with_min_price")]
        public IEnumerable<Article> GetArticlesWithMinPrice(string url)
        {
            var result = repository.LoadData(url);
            Response.WrapHeaderWithResult(result);
            return repository.GetArticlesWithMinPrice();
        }

        [HttpGet]
        [Route("articles_by_price")]
        public IEnumerable<Article> GetArticlesByPrice(string url, decimal price)
        {
            var result = repository.LoadData(url);
            Response.WrapHeaderWithResult(result);
            return repository.GetArticlesByPrice(price);
        }

        [HttpGet]
        [Route("answer_to_all_questions")]
        public AnswerToAllQuestions GetAnswerToAllQuestions(string url, decimal price, string brandName)
        {
            var result = repository.LoadData(url);
            Response.WrapHeaderWithResult(result);
            var answer = new AnswerToAllQuestions
            {
                AllBrandNames       = repository.GetAllBrandNames(),
                ArticlesByBrandName = repository.GetArticlesByBrandName(brandName),
                ArticlesWithMaxPrice = repository.GetArticlesWithMaxPrice(),
                ArticlesWithMinPrice    = repository.GetArticlesWithMinPrice(),
                ArticlesByPrice   = repository.GetArticlesByPrice(price)
            };
            return answer;
        }
    }
}