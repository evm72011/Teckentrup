using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Gateway.Models
{
    /// <summary>
    /// Common interface for Brands repository
    /// </summary>
    public interface IBrandRepository
    {
        public OperationResult LoadData(string url);

        public IEnumerable<string> GetAllBrandNames();

        public IEnumerable<Article> GetArticlesByBrandName(string brandName);

        public IEnumerable<Article> GetArticlesWithMaxPrice();

        public IEnumerable<Article> GetArticlesWithMinPrice();

        public IEnumerable<Article> GetArticlesByPrice(decimal price);
    }
}
