namespace Phlank.Responder
{
    internal interface IProblemResponse
    {
        /// <summary>
        /// Details regarding an error encountered during the execution of the requested operation. <c>null</c> if no error occured.
        /// </summary>
        public Problem Problem { get; }
    }
}
