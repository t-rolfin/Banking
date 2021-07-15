using Banking.Core.Entities;
using Banking.Core.Enums;
using Banking.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.AccountTypeFactory
{
    public class SilverAccountType : IAccountType
    {
        public AccountTypeEnum AccountType { get; } = AccountTypeEnum.Silver;

        public AccountType GetAccountType()
        {
            var silverAccount = new AccountType("Gold");

            silverAccount.AddOperation(
                    OperationType.Withdrawal,
                    new Commission(1, 0)
                );

            silverAccount.AddOperation(
                    OperationType.Transfer,
                    new Commission(0, 0)
                );

            silverAccount.AddOperation(
                    OperationType.Deposit,
                    new Commission(0, 0)
                );

            return silverAccount;
        }
    }
}
