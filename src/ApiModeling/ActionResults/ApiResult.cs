using Microsoft.AspNetCore.Mvc;

namespace Phlank.ApiModeling
{
    public class ApiResult : JsonResult
    {
        internal ApiResult() : base(null) { }

        internal ApiResult(object response) : base(response) { }
    }
}
