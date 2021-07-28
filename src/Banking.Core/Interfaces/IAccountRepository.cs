using Banking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Core.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> UpdateAccountList(CancellationToken cancellationToken, params Account[] accounts);
        Task<Account> GetById(Guid accountId);
        Task<Account> GetByIBAN(string accountIBAN);
    }
}
