using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Models
{
    public class AccountListModel 
    {
        public AccountListModel()
        {
            Accounts = new();
        }
        public AccountListModel(List<AccountModel> accounts)
        {
            Accounts = accounts;
        }
        public AccountListModel(Guid clientId, List<AccountModel> accounts)
        {
            ClientId = clientId;
            Accounts = accounts;
        }

        public Guid ClientId { get; set; }
        public List<AccountModel> Accounts { get; set; }
    }

    public class AccountModel
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public int AccType { get; set; }
    }
}
