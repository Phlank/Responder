using System;
using System.Runtime.Serialization;

namespace Phlank.Responder
{
    [Serializable]
    internal class BaseApiErrorNotFoundException : Exception
    {
        private string v1;
        private string v2;

        public BaseApiErrorNotFoundException()
        {
        }

        public BaseApiErrorNotFoundException(string message) : base(message)
        {
        }

        public BaseApiErrorNotFoundException(string v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public BaseApiErrorNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BaseApiErrorNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}