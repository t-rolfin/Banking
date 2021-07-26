using Banking.Core.Entities;
using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.AccountTypeFactory
{
    public class BasicAccountType : IAccountType
    {
        public AccountTypeEnum AccountType { get; } = AccountTypeEnum.Basic;

        public AccountType GetAccountType()
        {
            var basicAccount = new AccountType("Basic", (int)AccountTypeEnum.Basic);

            basicAccount.AddOperation(
                    OperationType.Withdrawal,
                    new Commission(1.5f, 2)
                );

            basicAccount.AddOperation(
                    OperationType.Transfer,
                    new Commission(0, 0)
                );

            basicAccount.AddOperation(
                    OperationType.Deposit,
                    new Commission(1, 0)
                );

            return basicAccount;
        }
    }
}
