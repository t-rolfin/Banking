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
        Task Deposit(Guid clientId, Guid accountId, decimal value, CancellationToken cancellationToken);
        Task<Client> IdentifyClient(string cnp, string pin);
        Task<Client> RegisterClient(string cnp, string pin, string firstName, 
            string lastName, string address, AccountTypeEnum accountType, CurrencyType currencyType, CancellationToken cancellationToken = default);
        Task Withdrawal(Guid clientId, Guid accountId, decimal value, CancellationToken cancellationToken);
        Task<IReadOnlyList<Account>> GetUserAccounts(Guid id);
    }
}