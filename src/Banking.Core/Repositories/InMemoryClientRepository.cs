using Banking.Core.Entities;
using Banking.Core.Exceptions;
using Banking.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Client GetByCNP(string cnp)
        {
            var client = _clients.FirstOrDefault(x => x.CNP == cnp);

            return client;
        }

        public List<Account> GetClientAccounts(string cnp)
        {
            var client = GetByCNP(cnp);

            return client.Accounts.ToList();
        }

        public void UpdatePIN(Client client)
        {
            var dbClient = GetByCNP(client.CNP);

            dbClient.ChangePIN(client.PIN);
        }
    }
}
