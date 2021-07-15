using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.Exceptions
{

    [Serializable]
    public class InsufficientAmountException : Exception
    {
        public InsufficientAmountException() { }
        public InsufficientAmountException(string message) : base(message) { }
        public InsufficientAmountException(string message, Exception inner) : base(message, inner) { }
        protected InsufficientAmountException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
