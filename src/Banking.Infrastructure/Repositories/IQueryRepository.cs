using Banking.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Repositories
{
    public interface IQueryRepository
    {
        Task<AccountListModel> GetClientAccounts(Guid clientId);
        Task<TransactionListModel> GetAccountTransactions(Guid accountId);
    }
}
