using System;
using System.Net;

namespace Phlank.Responder.Extensions
{
    internal static class ExceptionExtensions
    {
        public static Problem ToProblem<TException>(this TException exception) where TException : Exception
        {
            return new Problem(
                HttpStatusCode.InternalServerError,
                title: exception.GetType().Name,
                detail: exception.Message);
        }
    }
}
