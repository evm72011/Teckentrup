using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Gateway.Models
{
    public class BrandsRepository
    {
        private IEnumerable<Brand> data;

        public BrandsRepository(IEnumerable<Brand> data)
        {
            this.data = data;
        }

        public IEnumerable<Article> GetAllArticles()
        {
            return data.SelectMany(brand => brand.Types).SelectMany(typ => typ.Articles);
        }

        public IEnumerable<string> GetAllBrandNames()
        {
            return data.Select(brand => brand.BrandName);
        }

        public IEnumerable<Article> GetArticlesByBrandName(string brandName)
        {
            var neededBrands = data.Where(data => data.BrandName == brandName);
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
