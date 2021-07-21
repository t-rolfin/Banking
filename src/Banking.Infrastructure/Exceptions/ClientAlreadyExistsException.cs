using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Exceptions
{

    [Serializable]
    public class ClientAlreadyExistsException : Exception
    {
        public ClientAlreadyExistsException() { }
        public ClientAlreadyExistsException(string message) : base(message) { }
        public ClientAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected ClientAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
