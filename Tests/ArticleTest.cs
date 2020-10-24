using Models;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var article = new Article();
            article.Container = "26 x 0.5L (Glas)";
            Assert.IsTrue(article.TotalVolume == 13);
        }
    }
}