using Banking.Core.Entities;
using Banking.Shared.Enums;
using System.Collections.Generic;

namespace Banking.Core
{
    public interface IFacade
    {
        void ChangeClientPIN(string cnp, string newPIN);
        void Deposit(string cnp, string iban, decimal value);
        bool IdentifyClient(string cnp, string pin);
        bool RegisterClient(string cnp, string pin, string firstName, string lastName, string address, AccountTypeEnum accountType, CurrencyType currencyType);
        void Withdrawal(string cnp, string iban, decimal value);
        List<Account> GetUserAccounts(string cnp);
    }
}