using System;
using System.Collections.Generic;
using System.Text;

namespace Phlank.Responder
{
    internal interface IProblemResponse
    {
        /// <summary>
        /// Information regarding operational errors.
        /// </summary>
        public Problem Problem { get; }
    }
}
