using Banking.Core.Enums;
using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.Entities
{
    public class Transaction
    {
        protected Transaction() { }

        public Transaction(decimal amount, Account sourceAccount, Account destinationAccount, 
            OperationType transactionType, CurrencyType currencyType )
        {
            Amount = amount;
            SourceAccount = sourceAccount;
            DestinationAccount = destinationAccount;
            TransactionType = transactionType;
            CurrencyType = currencyType;
            Date = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public virtual Account SourceAccount { get; set; }
        public virtual Account DestinationAccount { get; set; }
        public OperationType TransactionType { get; set; }
        public CurrencyType CurrencyType { get; set; }
    }
}
