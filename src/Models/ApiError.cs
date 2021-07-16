using System.Collections.Generic;

namespace Phlank.ApiModeling
{
    public class ApiError
    {
        public IEnumerable<string> Fields { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public ApiError()
        {
        }
    }
}