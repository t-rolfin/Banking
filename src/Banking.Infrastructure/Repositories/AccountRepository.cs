using Banking.Core.AccountTypeFactory;
using Banking.Core.Entities;
using Banking.Core.Interfaces;
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
    public class AccountRepository : IAccountRepository
    {
        private readonly ClientContext _context;
        private readonly AccountTypeProviderFactory _accountTypeFactory;

        public AccountRepository(ClientContext context, AccountTypeProviderFactory accountTypeFactory)
        {
            _context = context;
            _accountTypeFactory = accountTypeFactory;
        }

        public async Task<Account> GetByIBAN(string accountIBAN)
        {
            var account = await _context.Accounts.Where(x => x.IBAN == accountIBAN)
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync();

            SetAccountType(account);

            return account;
        }

        public async Task<Account> GetById(Guid accountId)
        {
            var account =  await _context.Accounts.Where(x => x.Id == accountId)
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync();

            SetAccountType(account);

            return account;
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


        void SetAccountType(Account account)
        {
            int accountType = (int)_context.Entry(account).Property("AccType").CurrentValue;

            account.SetAccountType(_accountTypeFactory.GetAccountTypeByType((AccountTypeEnum)accountType));
        }
    }
}
