using System.Collections.Generic;

namespace Phlank.Responder
{
    internal interface ISuccessResponse
    {
        /// <summary>
        /// Indicates success of the requested operation on the server. Equivalent to <c>Response.Problem == null</c>.
        /// </summary>
        bool IsSuccessful { get; }

        /// <summary>
        /// Additional fields the server may send in relation to the operation.
        /// </summary>
        IDictionary<string, object> Extensions { get; }
    }

    internal interface ISuccessResponse<T>
    {
        /// <summary>
        /// Information sent by the server to consume in other applications.
        /// </summary>
        T Data { get; }
    }
}