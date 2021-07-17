using Banking.Shared.Enums;

namespace Banking.Core
{
    public interface IFacade
    {
        void ChangeClientPIN(string cnp, string newPIN);
        void Deposit(string cnp, string iban, decimal value);
        bool IdentifyClient(string cnp, string pin);
        void RegisterClient(string cnp, string pin, string firstName, string lastName, string address, AccountTypeEnum accountType, CurrencyType currencyType);
        void Withdrawal(string cnp, string iban, decimal value);
    }
}