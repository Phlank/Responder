using Phlank.Responder.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Phlank.Responder
{
    internal static class BaseErrors
    {
        internal static readonly Dictionary<HttpStatusCode, ApiError> DefaultStatusCodeDictionary = new Dictionary<HttpStatusCode, ApiError>()
        {
            {
                HttpStatusCode.BadRequest,
                new ApiError(
                    HttpStatusCode.BadRequest,
                    HttpStatusCode.BadRequest.ToString(),
                    "The request could not be understood by the server.",
                    new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"))
            },
            {
                HttpStatusCode.PaymentRequired,
                new ApiError(
                    HttpStatusCode.PaymentRequired,
                    HttpStatusCode.PaymentRequired.ToString(),
                    "Reserved for future use.",
                    new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.2"))
            },
            {
                HttpStatusCode.Forbidden,
                new ApiError(
                    HttpStatusCode.Forbidden,
                    HttpStatusCode.Forbidden.ToString(),
                    "The server has refused to fulfill the request.",
                    new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3"))
            },
            {
                HttpStatusCode.BadRequest,
                new ApiError(
                    HttpStatusCode.BadRequest,
                    HttpStatusCode.BadRequest.ToString(),
                    "The request could not be understood by the server.",
                    new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"),
                    null)
            },
            {
                HttpStatusCode.BadRequest,
                new ApiError(
                    HttpStatusCode.BadRequest,
                    HttpStatusCode.BadRequest.ToString(),
                    "The request could not be understood by the server.",
                    new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"),
                    null)
            },
            {
                HttpStatusCode.BadRequest,
                new ApiError(
                    HttpStatusCode.BadRequest,
                    HttpStatusCode.BadRequest.ToString(),
                    "The request could not be understood by the server.",
                    new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"),
                    null)
            },
            {
                HttpStatusCode.BadRequest,
                new ApiError(
                    HttpStatusCode.BadRequest,
                    HttpStatusCode.BadRequest.ToString(),
                    "The request could not be understood by the server.",
                    new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"),
                    null)
            },
            {
                HttpStatusCode.BadRequest,
                new ApiError(
                    HttpStatusCode.BadRequest,
                    HttpStatusCode.BadRequest.ToString(),
                    "The request could not be understood by the server.",
                    new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"),
                    null)
            },
            {
                HttpStatusCode.BadRequest,
                new ApiError(
                    HttpStatusCode.BadRequest,
                    HttpStatusCode.BadRequest.ToString(),
                    "The request could not be understood by the server.",
                    new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"),
                    null)
            },
            {
                HttpStatusCode.BadRequest,
                new ApiError(
                    HttpStatusCode.BadRequest,
                    HttpStatusCode.BadRequest.ToString(),
                    "The request could not be understood by the server.",
                    new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"),
                    null)
            },
            { HttpStatusCode.Forbidden, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3") },
            { HttpStatusCode.NotFound, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4") },
            { HttpStatusCode.MethodNotAllowed, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.5") },
            { HttpStatusCode.NotAcceptable, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.6") },
            { HttpStatusCode.RequestTimeout, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.7") },
            { HttpStatusCode.Conflict, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8") },
            { HttpStatusCode.Gone, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.9") },
            { HttpStatusCode.LengthRequired, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.10") },
            { HttpStatusCode.RequestEntityTooLarge, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.11") },
            { HttpStatusCode.RequestUriTooLong, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.12") },
            { HttpStatusCode.UnsupportedMediaType, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.13") },
            { HttpStatusCode.ExpectationFailed, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.14") },
            { HttpStatusCode.UpgradeRequired, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.15") },
            { HttpStatusCode.InternalServerError, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1") },
            { HttpStatusCode.NotImplemented, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.2") },
            { HttpStatusCode.BadGateway, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.3") },
            { HttpStatusCode.ServiceUnavailable, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.4") },
            { HttpStatusCode.GatewayTimeout, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.5") },
            { HttpStatusCode.HttpVersionNotSupported, new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.6") }
        };

        public static ApiError FromStatusCode(HttpStatusCode statusCode)
        {
            if (!statusCode.IsError()) throw new ArgumentOutOfRangeException(nameof(statusCode));
            if (!DefaultStatusCodeDictionary.ContainsKey(statusCode)) return null;

            return DefaultStatusCodeDictionary[statusCode];
        }
    }
}
