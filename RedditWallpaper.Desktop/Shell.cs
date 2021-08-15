using System.Collections.Generic;
using System.Collections.ObjectModel;
using RedditWallpaper.Core.Models;
using RedditWallpaper.Desktop.Helpers;

namespace RedditWallpaper.Desktop
{
    public class Shell : ObservableObject
    {
        public Shell()
        {
            Status = new();
            Settings = UserSettings.Read();
            WallPapers = new();

            TimeSpanUnits = new()
            {
                "hour",
                "day",
                "week",
                "month",
                "year",
                "all"
            };
        }

        public UserSettings Settings { get; set; }
        public ProgramStatus Status { get; set; }
        public List<string> TimeSpanUnits { get; set; }
        public ObservableCollection<WallpaperData> WallPapers { get; set; }
        public PagingInfo Paging { get; set; }
    }

    public record PagingInfo(string Sub = null, string NextPageCode = null);
}
