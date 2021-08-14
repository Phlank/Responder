using System;
using System.Collections.Generic;
using System.Text;

namespace Phlank.ApiModeling
{
    /// <summary>
    /// Options for the ApiModeling dependency injection.
    /// </summary>
    public class ApiModelingOptions
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
    }
}
