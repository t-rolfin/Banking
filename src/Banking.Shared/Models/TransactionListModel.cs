using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Models
{
    public record TransactionListModel
    { }

    public record TransactionModel(int Id, DateTime Date, decimal Amount, string OperationType, string CurrencyType) { }
}
