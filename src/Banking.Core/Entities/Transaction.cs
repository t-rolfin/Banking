using Banking.Core.Enums;
using Banking.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.Entities
{
    public class Transaction
    {
        public Transaction(decimal amount, Account sourceAccount, Account destinationAccount, 
            OperationType transactionType, CurrencyType currencyType )
        {
            Amount = amount;
            SourceAccount = sourceAccount;
            DestinationAccount = destinationAccount;
            TransactionType = transactionType;
            CurrencyType = currencyType;
        }

        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public Account SourceAccount { get; set; }
        public Account DestinationAccount { get; set; }
        public OperationType TransactionType { get; set; }
        public CurrencyType CurrencyType { get; set; }
    }
}
