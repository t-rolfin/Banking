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
            var commitions = AccountType.Operations
                .Find(x => x.OperationType == OperationType.Withdrawal)
                .Commission;

            var commitionValue = (withdrawalValue * (decimal)(commitions.Percent) / 100) + (decimal)commitions.Fixed;

            this.Amount -= (withdrawalValue + commitionValue);

            return commitionValue;
        }
        
        public decimal Deposit(decimal depositedValue)
        {
            var commitions = AccountType.Operations
                .Find(x => x.OperationType == OperationType.Deposit).Commission;

            var commitionValue = (depositedValue * (decimal)(commitions.Percent / 100)) + (decimal)commitions.Fixed;

            Amount += (depositedValue - commitionValue);

            return commitionValue;
        }
    }
}
