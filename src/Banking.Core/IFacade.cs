using Banking.Core.Entities;
using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Core
{
    public interface IFacade
    {
        Task ChangeClientPIN(string cnp, string newPIN, CancellationToken cancellationToken);
        Task Deposit(string cnp, string iban, decimal value, CancellationToken cancellationToken);
        Task<bool> IdentifyClient(string cnp, string pin);
        Task<Client> RegisterClient(string cnp, string pin, string firstName, 
            string lastName, string address, AccountTypeEnum accountType, CurrencyType currencyType, CancellationToken cancellationToken = default);
        Task Withdrawal(string cnp, string iban, decimal value, CancellationToken cancellationToken);
        Task<IReadOnlyList<Account>> GetUserAccounts(int id);
    }
}