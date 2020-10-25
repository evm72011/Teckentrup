using Models;
using NUnit.Framework;

namespace ModelTests
{
    public class ArticleTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TotalVolumeTest()
        {
            var article = new Article();
            Assert.IsNull(article.TotalVolume);

            article.Container = "26 x 0.5L";
            Assert.IsNull(article.TotalVolume);

            article.Container = "2error6 x 0.5L (Glas)";
            Assert.IsNull(article.TotalVolume);

            article.Container = "26 x 1 (Glas)";
            Assert.IsNull(article.TotalVolume);

            article.Container = "26 x 0.5X (Glas)";
            Assert.IsNull(article.TotalVolume);

            article.Container = "26 x 0.5L (Glas)";
            Assert.IsTrue(article.TotalVolume == 13);
        }

        [Test]
        public void PricePerLiterTest()
        {
            var article = new Article();
            Assert.IsNull(article.PricePerLiter);

            article.Price = 10;
            article.Container = "2error6 x 0.5L (Glas)";
            Assert.IsNull(article.PricePerLiter);

            article.Container = "0 x 0.5L (Glas)";
            Assert.IsNull(article.PricePerLiter);

            article.Container = "100 x 0.5L (Glas)";
            Assert.AreEqual(0.2m, article.PricePerLiter);
        }
    }
}