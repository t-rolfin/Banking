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
    }
}
