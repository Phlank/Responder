using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Phlank.Responder
{
    internal static class ResponseConverters
    {
        public static Response ToEmptyResponse<T>(this SerializableResponse<T> serializableResponse) where T : class
        {
            var success = serializableResponse.Status == null;
            if (success)
            {
                return new Response()
                {
                    Extensions = serializableResponse.Extensions
                };
            }
            else
            {
                return new Response()
                {
                    Problem = new Problem(
                    serializableResponse.Status.Value,
                        serializableResponse.Title,
                        serializableResponse.Detail,
                        serializableResponse.Type,
                        serializableResponse.Instance,
                        serializableResponse.Extensions)
                };
            }
        }

        public static Response<T> ToResponse<T>(this SerializableResponse<T> serializableResponse) where T : class
        {
            var success = serializableResponse.Status == null;
            if (success)
            {
                return new Response<T>()
                {
                    Data = serializableResponse.Data,
                    Extensions = serializableResponse.Extensions
                };
            }
            else
            {
                var error = new Problem(
                    serializableResponse.Status.Value,
                        serializableResponse.Title,
                        serializableResponse.Detail,
                        serializableResponse.Type,
                        serializableResponse.Instance,
                        serializableResponse.Extensions);

                return new Response<T>() { Problem = error };
            }
        }

        public static SerializableResponse<T> ToSerializableResponse<T>(this Response<T> response) where T : class
        {
            var success = response.IsSuccessful;
            if (success)
            {
                return new SerializableResponse<T>()
                {
                    IsSuccessful = response.IsSuccessful,
                    Data = response.Data,
                    Extensions = response.Extensions
                };
            }
            else
            {
                return new SerializableResponse<T>()
                {
                    IsSuccessful = response.IsSuccessful,
                    Status = response.Problem.Status,
                    Title = response.Problem.Title,
                    Detail = response.Problem.Detail,
                    Type = response.Problem.Type,
                    Instance = response.Problem.Instance,
                    Extensions = response.Extensions
                };
            }
        }

        public static SerializableResponse<object> ToSerializableResponse(this Response response)
        {
            var success = response.IsSuccessful;
            if (success)
            {
                return new SerializableResponse<object>()
                {
                    IsSuccessful = response.IsSuccessful,
                    Extensions = response.Extensions
                };
            }
            else
            {
                return new SerializableResponse<object>()
                {
                    IsSuccessful = response.IsSuccessful,
                    Status = response.Problem.Status,
                    Title = response.Problem.Title,
                    Detail = response.Problem.Detail,
                    Type = response.Problem.Type,
                    Instance = response.Problem.Instance,
                    Extensions = response.Extensions
                };
            }
        }
    }
}
