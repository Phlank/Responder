using System.Collections.Generic;

namespace Phlank.Responder
{
    internal interface ISuccessResponse<T> where T : class
    {
        T Data { get; }
        bool IsSuccessful { get; }
        IDictionary<string, object> Extensions { get; }
    }
}