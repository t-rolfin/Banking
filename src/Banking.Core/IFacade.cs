using Banking.Core.Entities;
using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rolfin.Result;

namespace Banking.Core
{
    public interface IFacade
    {
        Task<Result<bool>> Withdrawal(Guid clientId, Guid accountId, decimal value, CancellationToken cancellationToken);
        Task<Result<bool>> Deposit(Guid clientId, Guid accountId, decimal value, CancellationToken cancellationToken);
        Task<Result<bool>> Transfer(Guid accountId, string destinationIban, decimal value, CancellationToken cancellationToken);
        Task<Result<Client>> RegisterClient(string cnp, string pin, string firstName, 
            string lastName, string address, AccountTypeEnum accountType, CurrencyType currencyType, CancellationToken cancellationToken = default);
        Task<Client> IdentifyClient(string cnp, string pin);
        Task<Result<bool>> ChangeClientPIN(string cnp, string newPIN, CancellationToken cancellationToken);
        Task<Account> CreateAccountFor(Guid clientId, AccountTypeEnum accountType, CurrencyType currencyType, CancellationToken cancellationToken);
        Task<Result<bool>> CloseAccount(Guid clientId, Guid accountId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Account>> GetUserAccounts(Guid id);
    }
}