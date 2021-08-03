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
        Task<TransactionListModel> GetAccountTransactionsBetween(Guid accountId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<ClientModel>> GetClients();
        Task<IEnumerable<ClientModel>> GetClientsByName(string name);
        Task<IEnumerable<ClientModel>> GetClientsSortedByName(string searchedName, string sorted);
        Task<IEnumerable<ClientModel>> GetClientsSortedByAmount(string searchedName, string sorted);
    }
}
