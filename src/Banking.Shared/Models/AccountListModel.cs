using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Models
{
    public record AccountListModel(List<AccountModel> Accounts) { }

    public class AccountModel
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public int AccType { get; set; }
    }
}
