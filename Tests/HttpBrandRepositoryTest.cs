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
            repo.LoadDataFromJson("products.json");
        }

        [Test]
        public void GetAllBrandNames()
        {
            Assert.AreEqual(repo.GetAllBrandNames().Count(), 7);
        }

        [Test]
        public void GetArticlesByBrandName()
        {
            var articles = repo.GetArticlesByBrandName("AmIgO");
            Assert.AreEqual(articles.Count(), 1);
            Assert.AreEqual(articles.First().Id, 621);
        }

        [Test]
        public void GetArticlesWithMaxPrice()
        {
            var articles = repo.GetArticlesWithMaxPrice();
            Assert.AreEqual(articles.Count(), 1);
            Assert.AreEqual(articles.First().Id, 635);
        }

        [Test]
        public void GetArticlesWithMinPrice()
        {
            var articles = repo.GetArticlesWithMinPrice();
            Assert.AreEqual(articles.Count(), 1);
            Assert.AreEqual(articles.First().Id, 629);
        }

        [Test]
        public void GetArticlesByPrice()
        {
            var articles = repo.GetArticlesByPrice(11.99m);
            Assert.AreEqual(articles.Count(), 1);
            Assert.AreEqual(articles.First().Id, 621);
        }

    }
}
