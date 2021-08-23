using Banking.Core.Entities;
using Banking.Core.Exceptions;
using Banking.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Core.Repositories
{
    public class InMemoryClientRepository : IClientRepository
    {
        List<Client> _clients;

        public InMemoryClientRepository()
        {
            _clients = new();
        }

        public void Add(Client entity)
        {
            _clients.Add(entity);
        }

        public async Task<bool> CreateAsync(Client entity, CancellationToken cancellationToken)
        {
            return await Task.Factory.StartNew(() =>
            {
                _clients.Add(entity);

                return true;
            });
        }

        public Client GetByCNP(string cnp)
        {
            var client = _clients.FirstOrDefault(x => x.CNP == cnp);

            return client;
        }

        public async Task<Client> GetByCNPAsync(string cnp)
        {
            return await Task.Factory.StartNew(() =>
            {
                var client = _clients.FirstOrDefault(x => x.CNP == cnp);

                return client == default ? null : client;
            });
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            return await Task.Factory.StartNew(() =>
            {
                var client =_clients.FirstOrDefault(x => x.Id == id);
                return client == default ? null : client;
            });
        }

        public List<Account> GetClientAccounts(string cnp)
        {
            var client = GetByCNP(cnp);

            return client.Accounts.ToList();
        }

        public async Task<bool> UpdateAsync(Client entity, CancellationToken cancellationToken)
        {
            return await Task.Factory.StartNew(() =>
            {
                if(_clients.Any(x => x.Id == entity.Id))
                {
                    var client = _clients.First(x => x.Id == entity.Id);
                    _clients.Remove(client);
                    _clients.Add(entity);

                    return true;
                }
                else
                    return false;
            });
        }

        public void UpdatePIN(Client client)
        {
            var dbClient = GetByCNP(client.CNP);

            dbClient.ChangePIN(client.PIN);
        }

        public async Task<IReadOnlyList<Account>> GetClientAccountsById(Guid id)
        {
            return await Task.Factory.StartNew(() =>
            {
                var client = _clients.FirstOrDefault(x => x.Id == id);
                return client == default
                    ? null
                    : client.Accounts;
            });
        }
    }
}
