using System.Collections.Generic;

namespace Phlank.Responder
{
    internal interface ISuccessResponse
    {
        /// <summary>
        /// The success of the response.
        /// </summary>
        bool IsSuccessful { get; }

        /// <summary>
        /// Additional information sent by the server.
        /// </summary>
        IDictionary<string, object> Extensions { get; }
    }

    internal interface ISuccessResponse<T>
    {
        /// <summary>
        /// Data sent by the server.
        /// </summary>
        T Data { get; }
    }
}