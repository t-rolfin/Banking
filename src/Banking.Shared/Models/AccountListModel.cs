using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Models
{
    public record AccountListModel(List<AccountModel> Accounts) { }

    public record AccountModel(decimal Amount, CurrencyType CurrencyType, string AccountType) { }
}
