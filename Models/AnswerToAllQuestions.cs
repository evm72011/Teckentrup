﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class AnswerToAllQuestions
    {
        public IEnumerable<string> AllBrandNames { get; set; }
        public IEnumerable<Article> ArticlesByBrandName { get; set; }
        public IEnumerable<Article> ArticlesWithMaxPrice { get; set; }
        public IEnumerable<Article> ArticlesWithMinPrice { get; set; }
        public IEnumerable<Article> ArticlesByPrice { get; set; }
    }
}
