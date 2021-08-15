using System;

namespace RedditWallpaper.Desktop.Helpers
{
    public class VersionHelper
    {
        public static Version GetCurrent()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}
