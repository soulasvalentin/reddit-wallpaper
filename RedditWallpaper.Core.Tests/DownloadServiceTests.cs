using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RedditWallpaper.Core.Tests
{
    public class DownloadServiceTests
    {
        private DownloadService service;

        [SetUp]
        public void Setup()
        {
            // workingDir = current
            service = new DownloadService();
        }

        [Test]
        public void DownloadStatic()
        {
            var url = "http://i.imgur.com/ZPHmK4q.jpg"; // r/wallpaper top#1 all

            var localPath = service.DownloadImage(url);

            Assert.True(File.Exists(localPath));
        }

        [Test]
        public void NonExistantWorkingDir()
        {
            var workingDir = Path.Join(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString());

            Assert.Throws(typeof(Exception), () =>
            {
                var s = new DownloadService(workingDir);
            });
        }

        [Test]
        public void ExistantWorkingDir()
        {
            var workingDir = Path.Join(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(workingDir);

            Assert.DoesNotThrow(() =>
            {
                var s = new DownloadService(workingDir);
            });

            Directory.Delete(workingDir);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "reddit_wp_*.jpg")
                .ToList()
                .ForEach(x => File.Delete(x));
        }
    }
}