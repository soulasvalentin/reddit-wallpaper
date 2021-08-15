using Newtonsoft.Json;
using RedditWallpaper.Core.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RedditModels = RedditWallpaper.Core.Models.Reddit;

namespace RedditWallpaper.Core
{
    public class RedditService
    {        
        private const string BaseUrl = "https://www.reddit.com/r/";
        private const string ListingRelativeUrl = "{0}/top/.json?limit={1}&t={2}&after={3}";

        public List<WallpaperData> RetrievePosts(PostRetrievalParams @params, out string after)
        {
            var cli = new RestClient(BaseUrl);
            var req = new RestRequest(string.Format(ListingRelativeUrl, @params.Sub, @params.Limit, @params.TimeSpanFilter, @params.After));

            var res = cli.Execute<RedditModels.Body>(req);
            if (res.StatusCode != HttpStatusCode.OK)
                throw new Exception("Failed to retrieve posts");

            var body = JsonConvert.DeserializeObject<RedditModels.Body>(res.Content);

            after = body.Data.After;

            return ToWallpaperData(body.Data.Children);
        }

        /// <summary>
        /// Transform raw reddit response to usable type
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        private List<WallpaperData> ToWallpaperData(List<RedditModels.Child> children)
        {
            return children
                .Where(x => x.Data.Preview is not null)
                .Select(x => new WallpaperData(
                x.Data.Id, 
                x.Data.Title, 
                x.Data.Thumbnail,
                x.Data.Preview.Images.First().Source.Width,
                x.Data.Preview.Images.First().Source.Height,
                x.Data.Url,
                x.Data.Permalink
            )).ToList();
        }
    }

    public record PostRetrievalParams(string Sub, int Limit = 10, string TimeSpanFilter = TimeSpanFilter.All, string After = null);

    public class TimeSpanFilter
    {
        public const string Hour = "hour";
        public const string Day = "day";
        public const string Week = "week";
        public const string Month = "month";
        public const string Year = "year";
        public const string All = "all";
    }
}
