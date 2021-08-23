using Banking.Core.Entities;
using Banking.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Core.Repositories
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        public Task<Account> GetByIBAN(string accountIBAN)
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetById(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAccountList(CancellationToken cancellationToken, params Account[] accounts)
        {
            throw new NotImplementedException();
        }
    }
}
