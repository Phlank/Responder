using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Phlank.ApiModeling.Extensions
{
    internal static class HttpStatusCodeExtensions
    {
        public static bool IsSuccessful(this HttpStatusCode code)
        {
            if ((int)code >= 200 && (int)code < 300)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
