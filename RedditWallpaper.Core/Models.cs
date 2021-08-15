using System.Collections.Generic;

namespace RedditWallpaper.Core.Models.Reddit
{
    public record ImageSource(long Width, long Height);
    public record Image(ImageSource Source);
    public record Preview(List<Image> Images);
    public record ChildData(Preview Preview, string Title, string Thumbnail, string Url, string Permalink, string Post_hint, string Id);
    public record Child(ChildData Data);
    public record Data(List<Child> Children, string After);
    public record Body(Data Data);
}

namespace RedditWallpaper.Core.Models
{
    public record WallpaperData(string Id, string Title, string ThumbnailUrl, long Width, long Height, string FullImageUrl, string Permalink);
    public record ScreenSize(long Width, long Height);
}