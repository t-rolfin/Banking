using Banking.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.ClientAggregate
{
    public class Account
    {
        public Account(string iban, AccountType accountType, CurrencyType currencyType)
        {
            Id = Guid.NewGuid();
            IBAN = iban;
            AccountType = accountType;
            CurrencyType = currencyType;
        }

        public Guid Id { get; set; }
        public string IBAN { get; set; }
        public AccountType AccountType { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public decimal Amount { get; set; }

        public decimal Withdrawal(decimal withdrawalValue)
        {
            var commission = AccountType.Operations
                .Find(x => x.OperationType == OperationType.Withdrawal)
                .Commission;

            var commissionValue = (withdrawalValue * (decimal)(commission.Percent) / 100) + (decimal)commission.Fixed;

            this.Amount -= (withdrawalValue + commissionValue);

            return commissionValue;
        }
        
        public decimal Deposit(decimal depositedValue)
        {
            var commissions = AccountType.Operations
                .Find(x => x.OperationType == OperationType.Deposit).Commission;

            var commissionValue = (depositedValue * (decimal)(commissions.Percent / 100)) + (decimal)commissions.Fixed;

            Amount += (depositedValue - commissionValue);

            return commissionValue;
        }
    }
}
