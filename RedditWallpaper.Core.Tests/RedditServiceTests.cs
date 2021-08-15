using NUnit.Framework;
using System.Linq;

namespace RedditWallpaper.Core.Tests
{
    public class RedditServiceTests
    {
        private RedditService service;

        [SetUp]
        public void Setup()
        {
            service = new RedditService();
        }

        [Test]
        public void ListDefault()
        {
            PostRetrievalParams @params = new("wallpaper");
            var posts = service.RetrievePosts(@params, out string after);

            Assert.NotNull(posts);
            Assert.AreEqual(posts.Count, 10, message: "Items do not match");
        }


        [Test]
        public void ListDefaultPagination()
        {
            PostRetrievalParams @paramsPage1 = new("wallpaper");
            var postsPage1 = service.RetrievePosts(@paramsPage1, out string after1);

            PostRetrievalParams @paramsPage2 = new("wallpaper", After: after1);
            var postsPage2 = service.RetrievePosts(@paramsPage2, out string after2);

            // check page1 results are missing in page2
            Assert.False(postsPage2.Any(x => x.Id == postsPage1.First().Id));
            Assert.False(postsPage2.Any(x => x.Id == postsPage1.Last().Id));
        }
    }
}