using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.Exceptions
{

    [Serializable]
    public class AccountTypeCanNotBeSetException : Exception
    {
        public AccountTypeCanNotBeSetException() { }
        public AccountTypeCanNotBeSetException(string message) : base(message) { }
        public AccountTypeCanNotBeSetException(string message, Exception inner) : base(message, inner) { }
        protected AccountTypeCanNotBeSetException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
