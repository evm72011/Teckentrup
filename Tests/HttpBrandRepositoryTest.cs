using NUnit.Framework;
using Gateway.Models;
using System.Linq;

namespace GatewayTests
{
    class HttpBrandRepositoryTest
    {
        private HttpBrandRepositoryMock repo;
        [SetUp]
        public void Setup()
        {
            repo = new HttpBrandRepositoryMock();
            repo.LoadDataFromJson("testbrands.json");
        }

        [Test]
        public void GetAllBrandNames()
        {
            Assert.AreEqual(repo.GetAllBrandNames().Count(), 3);
        }

        [Test]
        public void GetArticlesByBrandName()
        {
            var articles = repo.GetArticlesByBrandName("BrAnD2");
            Assert.AreEqual(articles.Count(), 1);
            Assert.AreEqual(articles.First().Id, 112);
        }

        [Test]
        public void GetArticlesByBrandNameNull()
        {
            var articles = repo.GetArticlesByBrandName(null);
            Assert.AreEqual(articles.Count(), 0);
        }

        [Test]
        public void GetArticlesWithMaxPrice()
        {
            var articles = repo.GetArticlesWithMaxPrice();
            Assert.AreEqual(articles.Count(), 1);
            Assert.AreEqual(articles.First().Id, 111);
        }

        [Test]
        public void GetArticlesWithMinPrice()
        {
            var articles = repo.GetArticlesWithMinPrice();
            Assert.AreEqual(articles.Count(), 1);
            Assert.AreEqual(articles.First().Id, 112);
        }

        [Test]
        public void GetArticlesByPrice()
        {
            var articles = repo.GetArticlesByPrice(20.20m);
            Assert.AreEqual(articles.Count(), 1);
            Assert.AreEqual(articles.First().Id, 113);
        }

    }
}
