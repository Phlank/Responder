﻿using System;
using System.Collections.Generic;
using System.Net;

namespace Phlank.Responder
{
    public interface IApiError
    {
        /// <summary>
        /// A short, human-readable summary of the problem type.It SHOULD NOT
        /// change from occurrence to occurrence of the problem, except for 
        /// purposes of localization (e.g., using proactive content 
        /// negotiation).
        /// </summary>
        public string Detail { get; }

        /// <summary>
        /// A URI reference that identifies the specific occurrence of the 
        /// problem. It may or may not yield further information if 
        /// dereferenced.
        /// </summary>
        public Uri Instance { get; }

        /// <summary>
        /// The HTTP status code generated by the origin server for this
        /// occurence of the problem.
        /// </summary>
        public HttpStatusCode Status { get; }

        /// <summary>
        /// A human-readable explanation specific to this occurrence of the
        /// problem.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// A URI reference 
        /// [<see href="https://www.rfc-editor.org/rfc/rfc3986">RFC3986</see>]
        /// that identifies the problem type. RFC7807 encourages that, when 
        /// dereferenced, it provide human-readable documentation for the 
        /// problem type. When this member is not present, its value is assumed
        /// to be "about:blank".
        /// </summary>
        public Uri Type { get; }

        /// <summary>
        /// Problem type definitions MAY extend the problem details object with 
        /// additional members.
        /// </summary>
        public IDictionary<string, object> Extensions { get; }
    }
}