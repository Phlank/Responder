using System;
using System.Collections.Generic;
using System.Text;

namespace Phlank.Responder
{
    /// <summary>
    /// Options for ApiModeling dependency injection.
    /// </summary>
    public class ResponderOptions
    {
        /// <summary>
        /// The configuration string for Responder's options.
        /// </summary>
        public const string ApiModeling = "Phlank.ApiModeling";

        /// <summary>
        /// Property that determines whether the default
        /// InvalidModelStateResponseFactory is used or if Responder's
        /// InvalidModelStateResponseFactory is used.
        /// <para>
        /// Default is false.
        /// </para>
        /// </summary>
        public bool UseResponderInvalidModelStateResponseFactory { get; set; } = false;

        /// <summary>
        /// The character set used when writing a response using the
        /// <see cref="ApiResult"/>.
        /// <para>
        /// Default is "utf-8".
        /// </para>
        /// </summary>
        public string CharSet { get; set; } = "utf-8";
    }
}
