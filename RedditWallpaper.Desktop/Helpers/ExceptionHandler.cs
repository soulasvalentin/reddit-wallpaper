using System;
using System.Net.Http;

namespace RedditWallpaper.Desktop.Helpers
{
    public class ExceptionHandler
    {
        public static void Handle(Exception ex, string friendlyError = "Ha ocurrido un error inesperado. Contactesé con soporte técnico")
        {
            if (ex is BusinessException)
                MsgBox.Warning(ex.Message);
            else
            {
#if DEBUG
                MsgBox.Error("DEBUG: " + friendlyError + "\n\r\n\r" + ex.ToString());
#else
                MsgBox.Error(friendlyError);
#endif
                // todo: log
                // todo: mail
            }
        }
        public static void HandleHttpResponse(HttpResponseMessage res)
        {
            if (res.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new BusinessException(res.Content.ReadAsStringAsync().Result);
            else
                throw new Exception($"{res.StatusCode} - {res.RequestMessage.RequestUri} - {res.Content.ReadAsStringAsync().Result}");
        }
    }
}
