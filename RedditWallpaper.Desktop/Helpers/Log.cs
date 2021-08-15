using System;
using System.Diagnostics;
using System.IO;

namespace RedditWallpaper.Desktop.Helpers
{
    public static class Log
    {
        public static void Info(string info)
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/logs"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/logs");
            }
            try
            {
                Console.WriteLine("Info: " + info);
                File.AppendAllText("/logs/info.log", "<" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() +
                    "> " + info + Environment.NewLine);
            }
            catch { }
        }
        public static void Error(string Error)
        {
#if DEBUG
            MsgBox.Error(Error, "DEBUG ERROR");
#endif
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/logs"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/logs");
            }
            try
            {
                Console.WriteLine("Error: " + Error);
                File.AppendAllText("logs/error.log", "<" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() +
                    "> " + Error + Environment.NewLine);
            }
            catch { }
        }

        private static string ParentMethodName
        {
            get
            {
                StackTrace stackTrace = new StackTrace();
                return stackTrace.GetFrame(2).GetMethod().Name;
            }
        }
    }
}
