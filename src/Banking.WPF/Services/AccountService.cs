using Banking.Core;
using Banking.Infrastructure.Repositories;
using Banking.Shared.Enums;
using Banking.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.WPF.Services
{
    public class AccountService : IAccountService
    {
        private readonly IFacade _facade;
        private IQueryRepository _queryRepository;

        public AccountService(IFacade facade, IQueryRepository queryRepository)
        {
            _facade = facade;
            _queryRepository = queryRepository;
        }

        public async Task<bool> Deposit(Guid clientId, Guid accountId, decimal value)
        {
            await _facade.Deposit(clientId, accountId, value, default(CancellationToken));
            return true;
        }

        public async Task<ObservableCollection<Account>> GetClientAccounts(Guid ClientId)
        {
            var result = await _queryRepository.GetClientAccounts(ClientId);
            ObservableCollection<Account> listOfAccounts = new();
            foreach (var account in result.Accounts)
            {
                listOfAccounts.Add(
                        new Account()
                        {
                            Id = account.Id,
                            CardType = ((AccountTypeEnum)account.AccType).ToString(),
                            Sold = account.Amount
                        });
            }

            return listOfAccounts;
        }

        public async Task<bool> Withdrawal(Guid clientId, Guid accountId, decimal value)
        {
            await _facade.Withdrawal(clientId, accountId, value, default(CancellationToken));
            return true;
        }
    }
}
