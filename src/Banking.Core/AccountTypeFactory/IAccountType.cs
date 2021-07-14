using Banking.Core.ClientAggregate;
using Banking.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.AccountTypeFactory
{
    public interface IAccountType
    {
        public AccountTypeEnum AccountType { get; }
        AccountType GetAccountType();
    }
}
