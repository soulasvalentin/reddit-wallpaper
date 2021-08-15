using System.Windows;

namespace RedditWallpaper.Desktop.Helpers
{
    public static class MsgBox
    {
        public static void Info(string msg, string title = "Information")
        {
            MessageBox.Show(msg, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void Error(string msg, string title = "Error")
        {
            MessageBox.Show(msg, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Warning(string msg, string title = "Warning")
        {
            MessageBox.Show(msg, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static bool QuestionYesNo(string msg, string title = "Question")
        {
            return MessageBox.Show(msg, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public static bool? QuestionYesNoCancel(string msg, string title = "Question")
        {
            MessageBoxResult r = MessageBox.Show(msg, title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (r == MessageBoxResult.Yes)
                return true;
            else if (r == MessageBoxResult.No)
                return false;
            else
                return null;
        }
    }
}
