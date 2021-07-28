using Banking.Core.Entities;
using Banking.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ClientContext _context;

        public AccountRepository(ClientContext context)
        {
            _context = context;
        }

        public async Task<Account> GetByIBAN(string accountIBAN)
        {
            return await _context.Accounts.Where(x => x.IBAN == accountIBAN)
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync();
        }

        public async Task<Account> GetById(Guid accountId)
        {
            return await _context.Accounts.Where(x => x.Id == accountId)
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAccountList(CancellationToken cancellationToken, params Account[] accounts)
        {
            if (accounts is null)
                return false;

            foreach (var account in accounts)
            {
                var state = _context.Entry(account).State;
            }

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 1 ? true : false;
        }
    }
}
