using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phlank.ApiModeling
{
    public class ApiResult : JsonResult
    {
        internal ApiResult(ApiResponse response) : base(response)
        {

        }
    }
}
