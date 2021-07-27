using Banking.Core.AccountTypeFactory;
using Banking.Core.Entities;
using Banking.Core.Interfaces;
using Banking.Infrastructure.Exceptions;
using Banking.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientContext _context;
        private readonly AccountTypeProviderFactory _accountTypeFactory;


        public ClientRepository(ClientContext context, AccountTypeProviderFactory accountTypeFactory)
        {
            _context = context;
            _accountTypeFactory = accountTypeFactory;
        }


        public async Task<bool> CreateAsync(Client entity, CancellationToken cancellationToken)
        {
            try
            {
                if (CheckIfClientExistsByCNP(entity.CNP))
                    throw new ClientAlreadyExistsException();

                if (entity is null || entity == default)
                    throw new ArgumentNullException();

                foreach (var account in entity.Accounts)
                {
                    _context.Entry(account).Property("AccType").CurrentValue = account.AccountType.EnumPosition;
                }

                await _context.AddAsync(entity);

                var response = await _context.SaveChangesAsync(cancellationToken);

                return response > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> UpdateAsync(Client entity, CancellationToken cancellationToken)
        {
            try
            {
                if (entity is null || entity == default)
                    throw new ArgumentNullException();

                if(entity.HasNewAccount)
                {
                    foreach(var account in entity.Accounts.Where(x => x.IsNew == true))
                    {
                        _context.Entry(account).State = EntityState.Added;
                        _context.Entry(account).Property("AccType").CurrentValue = account.AccountType.EnumPosition;
                    }
                }
                else
                {
                    _context.Entry(entity).State = EntityState.Modified;
                }

                var response = await _context.SaveChangesAsync(cancellationToken);

                return response > 1 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IReadOnlyList<Account>> GetClientAccountsById(Guid id)
        {
            var client = await GetByIdAsync(id);

            return client.Accounts;
        }
        public async Task<Client> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentOutOfRangeException();

            var client = await _context.Clients.FindAsync(id);

            if (client is not null)
            {
                foreach (var account in client?.Accounts)
                {
                    int accountType = (int)_context.Entry(account).Property("AccType").CurrentValue;

                    account.SetAccountType(_accountTypeFactory.GetAccountTypeByType((AccountTypeEnum)accountType));
                }
            }

            return client;
        }
        public async Task<Client> GetClientByCNPAsync(string cnp)
        {
            try
            {
                var clients = _context.Clients.Where(x => x.CNP == cnp);
                var client = clients.Count() == 0 ? null : await clients.FirstAsync();

                if(client is not null)
                {
                    foreach (var account in client?.Accounts)
                    {
                        int accountType = (int)_context.Entry(account).Property("AccType").CurrentValue;

                        account.SetAccountType(_accountTypeFactory.GetAccountTypeByType((AccountTypeEnum)accountType));
                    }
                }

                return client;
            }
            catch
            {
                throw;
            }
        }



        private bool CheckIfClientExistsByCNP(string cnp)
        {
            return _context.Clients.Any(x => x.CNP == cnp);
        }
    }
}
