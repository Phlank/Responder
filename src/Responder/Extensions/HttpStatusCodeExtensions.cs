using System.Net;

namespace Phlank.Responder.Extensions
{
    internal static class HttpStatusCodeExtensions
    {
        public static bool IsSuccessful(this HttpStatusCode code)
        {
            if ((int)code >= 200 && (int)code < 300)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsError(this HttpStatusCode code)
        {
            if ((int)code >= 400)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
