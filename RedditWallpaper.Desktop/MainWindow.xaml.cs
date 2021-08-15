using RedditWallpaper.Core;
using RedditWallpaper.Core.Models;
using RedditWallpaper.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RedditWallpaper.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Shell vm;

        private BackgroundWorker wkGetPosts, wkSetImage;

        private RedditService redditService;
        private DownloadService downloadService;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = vm = new();

            redditService = new();
            downloadService = new();

            Title = "Reddit Wallpaper Picker V" + VersionHelper.GetCurrent();

            wkGetPosts = new();
            wkGetPosts.DoWork += WkGetPosts_DoWork;
            wkGetPosts.RunWorkerCompleted += WkGetPosts_RunWorkerCompleted;

            wkSetImage = new();
            wkSetImage.DoWork += WkSetImage_DoWork;
            wkSetImage.RunWorkerCompleted += WkSetImage_RunWorkerCompleted;
        }

        #region BUTTONS
        private void BtnSearchClick(object sender, RoutedEventArgs e)
        {
            // TODO: remove force set & fix
            vm.Settings.Subreddit = cboxSubreddits.Text;
            Search();
        }

        private void BtnViewInBrowser(object sender, RoutedEventArgs e)
        {
            var wp = (sender as Button).DataContext as WallpaperData;
            if (!string.IsNullOrWhiteSpace(wp.Permalink))
            {
                var fullUrl = $"http://www.reddit.com{wp.Permalink}";
                System.Diagnostics.Process.Start("explorer.exe", fullUrl);
            }
        }
        
        private void BtnDownloadWallpaper(object sender, RoutedEventArgs e)
        {
            var wp = (sender as Button).DataContext as WallpaperData;
            if (!string.IsNullOrWhiteSpace(wp.FullImageUrl))
            {
                vm.Status.Set(true, "Downloading...");

                // second param means if it will be set as wallpaper
                wkSetImage.RunWorkerAsync((wp, false, string.Empty));
            }
        }

        private void BtnSetWallpaper(object sender, RoutedEventArgs e)
        {
            var wp = (sender as Button).DataContext as WallpaperData;
            if (!string.IsNullOrWhiteSpace(wp.FullImageUrl))
            {
                vm.Status.Set(true, "Downloading...");

                // second param means if it will be set as wallpaper
                wkSetImage.RunWorkerAsync((wp, true, cboxStyles.SelectionBoxItem as string));
            }
        }

        private void BtnNextPageClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(vm.Settings.Subreddit) ||
                string.IsNullOrWhiteSpace(vm.Settings.TimeSpan) ||
                vm.Settings.Subreddit != vm.Paging.Sub) // if user changed the sub
                return;

            vm.Settings.Save();

            vm.Status.Set(true, "Searching...");

            wkGetPosts.RunWorkerAsync();
        }
        #endregion

        #region OTHER EVENTS
        private void Subreddit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // trigger lost focus to update binding
                cboxLastUnit.Focus();
                cboxSubreddits.Focus();

                Search();
            }
        }
        #endregion

        #region HANDLERS
        private void Search()
        {
            if (string.IsNullOrWhiteSpace(vm.Settings.Subreddit) ||
                string.IsNullOrWhiteSpace(vm.Settings.TimeSpan))
                return;

            vm.Settings.Subreddit = vm.Settings.Subreddit.ToLower().Trim();

            // if new sub, save
            if (!vm.Settings.Subreddits.Contains(vm.Settings.Subreddit))
            {
                vm.Settings.Subreddits.Insert(0, vm.Settings.Subreddit);
                // TODO: raise seems not to be working, fix
                vm.RaisePropertyChango(nameof(vm.Settings));
            }

            vm.Settings.Save();

            vm.Status.Set(true, "Searching...");
            vm.WallPapers.Clear();
            vm.Paging = new();

            wkGetPosts.RunWorkerAsync();
        }
        #endregion

        #region WORKERS
        private void WkGetPosts_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                PostRetrievalParams @params = new(
                    vm.Settings.Subreddit, 
                    TimeSpanFilter: vm.Settings.TimeSpan, 
                    After: vm.Paging.NextPageCode);
                e.Result = redditService.RetrievePosts(@params, out string after);
                vm.Paging = new(vm.Settings.Subreddit, after);
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void WkGetPosts_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            vm.Status.Set(false);
            if (e.Result is not Exception)
            {
                foreach (var wp in e.Result as List<WallpaperData>)
                    vm.WallPapers.Add(wp);

                btnNextPage.Visibility = vm.WallPapers.Count > 0 && vm.Paging.NextPageCode is not null
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                if (vm.WallPapers.Count == 0)
                    MsgBox.Warning($"No items were found for sub \"{vm.Settings.Subreddit}\"");
            }
        }

        private void WkSetImage_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // download
                var args = ((WallpaperData, bool, string))e.Argument;
                var localPath = downloadService.DownloadImage(args.Item1.FullImageUrl);

                // set wallpaper
                if (args.Item2)
                    ScreenSetter.SetWallpaper(localPath, args.Item3);
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void WkSetImage_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            vm.Status.Set(false);
            if (e.Result is Exception)
                ExceptionHandler.Handle(e.Result as Exception);
            else
            {

            }
        }
        #endregion
    }
}
