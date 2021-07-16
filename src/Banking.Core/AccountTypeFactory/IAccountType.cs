using Banking.Core.Entities;
using Banking.Shared.Enums;
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
