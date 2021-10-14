using System;
using System.Net;

namespace Phlank.Responder.Extensions
{
    internal static class ExceptionExtensions
    {
        public static ApiError ToApiError<TException>(this TException exception) where TException : Exception
        {
            return new ApiError(
                HttpStatusCode.InternalServerError,
                title: exception.GetType().Name,
                detail: exception.Message);
        }
    }
}
