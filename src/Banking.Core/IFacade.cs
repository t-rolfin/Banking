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
        Task Withdrawal(Guid clientId, Guid accountId, decimal value, CancellationToken cancellationToken);
        Task Deposit(Guid clientId, Guid accountId, decimal value, CancellationToken cancellationToken);
        Task Transfer(Guid accountId, string destinationIban, decimal value, CancellationToken cancellationToken);
        Task<Client> RegisterClient(string cnp, string pin, string firstName, 
            string lastName, string address, AccountTypeEnum accountType, CurrencyType currencyType, CancellationToken cancellationToken = default);
        Task<Client> IdentifyClient(string cnp, string pin);
        Task ChangeClientPIN(string cnp, string newPIN, CancellationToken cancellationToken);
        Task<Account> CreateAccountFor(Guid clientId, AccountTypeEnum accountType, CurrencyType currencyType, CancellationToken cancellationToken);
        Task<bool> CloseAccount(Guid clientId, Guid accountId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Account>> GetUserAccounts(Guid id);
    }
}