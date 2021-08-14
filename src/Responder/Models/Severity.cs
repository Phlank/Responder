namespace Phlank.Responder
{
    /// <summary>
    /// Represents the level of criticality regarding a warning.
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// Imminent failure of the operation.
        /// </summary>
        Critical = 1,
        /// <summary>
        /// A high level of concern either regarding the operation or its result.
        /// </summary>
        High = 2,
        /// <summary>
        /// A low level of concern either regarding the operation or its result.
        /// </summary>
        Low = 3
    }
}