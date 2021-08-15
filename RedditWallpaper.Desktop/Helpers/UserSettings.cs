using RedditWallpaper.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RedditWallpaper.Desktop.Helpers
{
    public class UserSettings
    {
        #region CUSTOMIZATION
        /// <summary>
        /// Sets the default values
        /// </summary>
        public UserSettings()
        {
            Subreddit = "wallpaper";
            Subreddits = BuildDefaultSubreddits();
            TimeSpan = TimeSpanFilter.All;
        }

        public string Subreddit { get; set; }
        public string TimeSpan { get; set; }
        public List<string> Subreddits { get; set; }

        private void AfterSuccessReadHandler()
        {
            if (Subreddits is null || Subreddits.Count == 0)
                Subreddits = BuildDefaultSubreddits();

            Subreddits = Subreddits.Distinct().ToList();
        }

        private List<string> BuildDefaultSubreddits()
        {
            return new()
            {
                "wallpaper",
                "wallpapers",
                "earthporn",
                "cityporn",
                "skyporn",
                "weatherporn",
                "botanicalporn",
                "lakeporn",
                "villageporn",
                "waterporn",
                "spaceporn",
                "multiwall",
            };
        }
        #endregion

        // IO
        public void Save()
        {
            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "/settings.xml"))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(UserSettings));
                xmls.Serialize(sw, this);
            }
        }
        public static UserSettings Read()
        {
            string cur = Directory.GetCurrentDirectory();
            if (File.Exists(cur + "/settings.xml"))
            {
                try
                {
                    using (StreamReader sw = new StreamReader(cur + "/settings.xml"))
                    {
                        XmlSerializer xmls = new XmlSerializer(typeof(UserSettings));
                        var localUs = xmls.Deserialize(sw) as UserSettings ?? new UserSettings();
                        localUs.AfterSuccessReadHandler();
                        return localUs;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("UserSettings.Read | " + ex.Message);

                    // Backup
                    if (File.Exists(cur + "/settings.xml"))
                    {
                        if (!Directory.Exists(cur + "/backups"))
                            Directory.CreateDirectory(cur + "/backups");
                        string nowString = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                        File.Copy(cur + "/settings.xml", cur + "/backups/settings_" + nowString + ".xml");
                        MsgBox.Info("An error has occurred while reading the user settings. A backup of the damaged file has been made in " + "\"/backups/settings_" + nowString + ".xml\"");
                    }
                }
            }
            else
            {
                var us = new UserSettings();
                us.Save();
                return us;
            }
            return new UserSettings();
        }
    }
}
