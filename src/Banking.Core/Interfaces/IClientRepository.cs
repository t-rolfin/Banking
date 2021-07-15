using Banking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Client GetByCNP(string cnp);
        void UpdatePIN(Client client);
    }
}
