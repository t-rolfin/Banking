using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Models
{
    public class CreateAccountModel
    {
        public CreateAccountModel() { }

        public CreateAccountModel(Guid clientId, CurrencyType currencyType, AccountTypeEnum accountType)
        {
            ClientId = clientId;
            CurrencyType = currencyType;
            AccountType = accountType;
        }

        public Guid ClientId { get; set; }
        public CurrencyType CurrencyType { get; set; } = CurrencyType.RON;
        public AccountTypeEnum AccountType { get; set; } = AccountTypeEnum.Basic;
    }
}
