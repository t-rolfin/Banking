using Banking.Shared.Helpers;
using Banking.Shared.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Banking.Infrastructure.Repositories
{
    public class ClientQueryRepository : IQueryRepository
    {

        readonly ConnectionString _connectionString;

        public ClientQueryRepository(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<TransactionListModel> GetAccountTransactions(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountListModel> GetClientAccounts(Guid clientId)
        {
            string query = $"SELECT * FROM accounts WHERE ClientId = @ClientId AND IsClosed = 0";

            using var connection = new SqlConnection(_connectionString.Value);
            await connection.OpenAsync();

            var result = await connection.QueryAsync<AccountModel>(query, new { ClientId = clientId });

            return new AccountListModel(result.ToList());
        }
    }
}
