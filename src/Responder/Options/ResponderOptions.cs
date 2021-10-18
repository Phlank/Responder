namespace Phlank.Responder
{
    /// <summary>
    /// Options for the behavior of the Responder library..
    /// </summary>
    public class ResponderOptions
    {
        /// <summary>
        /// The configuration string for the Responder library.
        /// </summary>
        public const string Responder = "Phlank.Responder";

        public bool IncludeTraceIdOnErrors { get; set; } = true;
    }
}
