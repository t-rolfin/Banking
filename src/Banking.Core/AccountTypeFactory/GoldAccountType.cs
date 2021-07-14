using Banking.Core.ClientAggregate;
using Banking.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.AccountTypeFactory
{
    public class GoldAccountType : IAccountType
    {
        public AccountTypeEnum AccountType { get; } = AccountTypeEnum.Gold;

        public AccountType GetAccountType()
        {
            var goldAccount = new AccountType("Gold");

            goldAccount.AddOperation(
                    OperationType.Withdrawal,
                    new Commission(0, 0)
                );

            goldAccount.AddOperation(
                    OperationType.Transfer,
                    new Commission(0, 0)
                );

            goldAccount.AddOperation(
                    OperationType.Deposit,
                    new Commission(0, 0)
                );

            return goldAccount;
        }
    }
}
