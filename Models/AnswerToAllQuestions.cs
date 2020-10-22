using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class AnswerToAllQuestions
    {
        public IEnumerable<string> AllBrandNames { get; set; }
        public IEnumerable<Article> ArticlesByBrandName { get; set; }
        public IEnumerable<Article> ExpensivestArticles { get; set; }
        public IEnumerable<Article> CheapestArticles { get; set; }
        public IEnumerable<Article> ArticlesWithPrice { get; set; }
    }
}
