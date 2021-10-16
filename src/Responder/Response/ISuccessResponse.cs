using System.Collections.Generic;

namespace Phlank.Responder
{
    internal interface ISuccessResponse
    {
        bool IsSuccessful { get; }
        IDictionary<string, object> Extensions { get; }
    }

    internal interface ISuccessResponse<T> : ISuccessResponse where T : class
    {
        T Data { get; }
    }
}