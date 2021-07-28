using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Models
{
    public class TransactionListModel
    {
        public TransactionListModel() { }

        public TransactionListModel(IEnumerable<TransactionModel> transactions)
        {
            Transactions = transactions;
        }

        public Guid AccountId { get; set; }
        public IEnumerable<TransactionModel> Transactions { get; set; }
    }

    public class TransactionModel {
        public TransactionModel() { }

        public TransactionModel(int id, DateTime date, decimal amount, Guid destinationAccountId, int transactionType, string currencyType)
        {
            Id = id;
            Date = date;
            Amount = amount;
            TransactionType = transactionType;
            CurrencyType = currencyType;
            DestinationAccountId = destinationAccountId;
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public Guid DestinationAccountId { get; set; }
        public int TransactionType { get; set; }
        public string CurrencyType { get; set; }
    }
}
