using System;
using System.Net;

namespace Phlank.Responder.Extensions
{
    internal static class ExceptionExtensions
    {
        public static ApiError ToApiError<TException>(this TException exception) where TException : Exception
        {
            var status = GetStatusCodeForException(exception);

            return new ApiError
            {
                Title = $"{exception.GetType().Name}",
                Detail = $"{exception.Message}",
                Status = status
            };
        }

        private static HttpStatusCode GetStatusCodeForException<TException>(TException exception) where TException : Exception
        {
            var exceptionType = typeof(TException);
            if (exceptionType == typeof(NotImplementedException)
                || exceptionType.IsSubclassOf(typeof(NotImplementedException)))
            {
                return HttpStatusCode.NotImplemented;
            }
            else
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
