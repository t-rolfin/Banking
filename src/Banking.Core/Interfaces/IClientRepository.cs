﻿using Banking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.Interfaces
{

    public interface IClientRepository : IAsyncRepository<Client>
    { }


    public interface IInMemoryClientRepository : IRepository<Client>
    {
        Client GetByCNP(string cnp);
        void UpdatePIN(Client client);
        List<Account> GetClientAccounts(string cnp);
    }
}
