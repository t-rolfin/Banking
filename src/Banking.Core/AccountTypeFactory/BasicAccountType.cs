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
    public class BasicAccountType : IAccountType
    {

        public AccountTypeEnum AccountType { get; } = AccountTypeEnum.Basic;

        public AccountType GetAccountType()
        {
            var basicAccount = new AccountType("Gold");

            basicAccount.AddOperation(
                    OperationType.Withdrawal,
                    new Commission(0, 0)
                );

            basicAccount.AddOperation(
                    OperationType.Transfer,
                    new Commission(0, 0)
                );

            basicAccount.AddOperation(
                    OperationType.Deposit,
                    new Commission(0, 0)
                );

            return basicAccount;
        }
    }
}
