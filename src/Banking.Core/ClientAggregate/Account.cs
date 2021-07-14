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
        public Guid Id { get; set; }
        public string IBAN { get; set; }
        public string PIN { get; protected set; }
        public AccountType AccountType { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public bool isClosed { get; }

        public void ChangePIN(string newPin)
            => this.PIN = newPin;
    }
}
