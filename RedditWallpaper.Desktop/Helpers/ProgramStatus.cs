using System.Windows.Media;

namespace RedditWallpaper.Desktop.Helpers
{
    public class ProgramStatus : ObservableObject
    {
        public ProgramStatus()
        {
            _message = "Ready";
        }
        private bool _working;
        public bool Working
        {
            get { return _working; }
            set
            {
                _working = value;
                RaisePropertyChango(nameof(Working));
                RaisePropertyChango(nameof(BarColor));
            }
        }

        public Brush BarColor
        {
            get
            {
                if (Working)
                    return new SolidColorBrush(Color.FromRgb(202, 81, 0));
                else
                    return new SolidColorBrush(Color.FromRgb(0, 122, 204));
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChango(nameof(Message));
            }
        }

        public void Set(bool _working, string _msgWork = "Working...", string _msgReady = "Ready")
        {
            Working = _working;
            if (Working)
                Message = _msgWork;
            else
                Message = _msgReady;
        }
    }
}
