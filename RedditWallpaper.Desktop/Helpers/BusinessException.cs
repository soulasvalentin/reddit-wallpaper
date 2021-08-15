using System;
using System.Runtime.Serialization;
using System.Windows;

namespace RedditWallpaper.Desktop.Helpers
{
    public class BusinessException : Exception
    {
        public BusinessException()
        {
            double height = SystemParameters.FullPrimaryScreenHeight;
            double width = SystemParameters.FullPrimaryScreenWidth;
            double resolution = height * width;
        }

        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
