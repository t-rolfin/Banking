﻿using Banking.Core.Entities;
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
        private List<Client> _clients;

        public void Add(Client entity)
        {
            _clients.Add(entity);
        }

        public Client GetByCNP(string cnp)
        {
            var client = _clients.FirstOrDefault(x => x.CNP == cnp);

            _ = client ?? throw new ClientNotFoundException("The client with specified CNP can not be found.");

            return client;
        }

        public void UpdatePIN(Client client)
        {
            var dbClient = GetByCNP(client.CNP);

            dbClient.ChangePIN(client.PIN);
        }
    }
}
