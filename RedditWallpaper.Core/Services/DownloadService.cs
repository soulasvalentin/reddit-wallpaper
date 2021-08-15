using RestSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RedditWallpaper.Core
{
    public class DownloadService
    {
        private readonly string workingDir;

        /// <param name="workingDir">Where images will be downloaded</param>
        public DownloadService(string workingDir = null)
        {
            if (!string.IsNullOrEmpty(workingDir))
            {
                if (!Directory.Exists(workingDir))
                    throw new Exception($"Working directory does not exists. dir={workingDir}");
                this.workingDir = workingDir;
            }
            else
                this.workingDir = Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// Downloads image from url in the current dir with a unique name and returns full path
        /// </summary>
        /// <param name="url">Image url to download</param>
        /// <returns>Full path to local image</returns>
        public string DownloadImage(string url)
        {
            var cli = new RestClient(url);
            var req = new RestRequest(string.Empty, Method.GET);
            var bytes = cli.DownloadData(req);
            var fileName = $"reddit_wp_{Guid.NewGuid().ToString().Substring(0, 8)}.jpg";
            var path = Path.Join(workingDir, fileName);
            File.WriteAllBytesAsync(path, bytes);
            return path;
        }
    }
}
