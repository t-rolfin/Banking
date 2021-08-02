using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.Exceptions
{

    [Serializable]
    public class AccountCanNotBeClosedException : Exception
    {
        public AccountCanNotBeClosedException() { }
        public AccountCanNotBeClosedException(string message) : base(message) { }
        public AccountCanNotBeClosedException(string message, Exception inner) : base(message, inner) { }
        protected AccountCanNotBeClosedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
