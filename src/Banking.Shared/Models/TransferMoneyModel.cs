using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Models
{
    public class TransferMoneyModel
    {
        public TransferMoneyModel() { }

        public TransferMoneyModel(string destinationAccountIBAN, decimal value, Guid accountId)
        {
            DestinationAccountIBAN = destinationAccountIBAN;
            AccountId = accountId;
            Value = value;
        }

        public Guid AccountId { get; set; }
        public string DestinationAccountIBAN { get; set; }
        public decimal Value { get; set; }
    }
}
