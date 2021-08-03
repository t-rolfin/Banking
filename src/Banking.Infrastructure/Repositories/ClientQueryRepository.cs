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

        public async Task<TransactionListModel> GetAccountTransactions(Guid accountId)
        {
            string query = $"SELECT * FROM transactions WHERE SourceAccountId = '{ accountId }'";

            using var connection = new SqlConnection(_connectionString.Value);

            await connection.OpenAsync();

            var result = await ExecuteQuery<TransactionModel>(query);

            return new TransactionListModel(result);
        }

        public async Task<TransactionListModel> GetAccountTransactionsBetween(Guid accountId, DateTime startDate, DateTime endDate)
        {
            string query = $"SELECT * FROM transactions WHERE SourceAccountId = '{ accountId }' " +
                $"AND Date BETWEEN '{startDate}' AND '{endDate}'";

            using var connection = new SqlConnection(_connectionString.Value);

            await connection.OpenAsync();

            var result = await ExecuteQuery<TransactionModel>(query);

            return new TransactionListModel(result);
        }

        public async Task<TransactionListModel> GetAccountTransactionsByIds(Guid accountId, string listOfIds)
        {
            string query = $"SELECT * FROM transactions WHERE SourceAccountId = '{ accountId }' " +
                $"AND Id in ({listOfIds})";

            var result = await ExecuteQuery<TransactionModel>(query);

            return new TransactionListModel(result);
        }

        public async Task<AccountListModel> GetClientAccounts(Guid clientId)
        {
            string query = $"SELECT * FROM accounts WHERE ClientId = @ClientId AND IsClosed = 0";

            using var connection = new SqlConnection(_connectionString.Value);
            await connection.OpenAsync();

            var result = await connection.QueryAsync<AccountModel>(query, new { ClientId = clientId });

            return new AccountListModel(result.ToList());
        }

        public async Task<IEnumerable<ClientModel>> GetClients()
        {
            string query = "SELECT Id, CNP, (FirstName + ' ' + LastName) as FullName, Address, Total FROM clients C " +
                "JOIN(SELECT ClientId, SUM(Amount) AS Total FROM accounts WHERE IsClosed = 0 GROUP BY ClientId) A ON C.Id = A.ClientId";

            return await ExecuteQuery<ClientModel>(query);
        }

        public async Task<IEnumerable<ClientModel>> GetClientsByName(string name)
        {
            var extraOption = string.IsNullOrWhiteSpace(name) 
                ? "" 
                : $" WHERE CONCAT(FirstName,' ', LastName) LIKE '%{name}%'";

            string query = "SELECT Id, CNP, concat(FirstName, ' ', LastName) as FullName, Address, Total FROM clients C " +
                "JOIN(SELECT ClientId, SUM(Amount) AS Total FROM accounts WHERE IsClosed = 0 GROUP BY ClientId) A ON C.Id = A.ClientId " +
                $"{extraOption}";

            return await ExecuteQuery<ClientModel>(query);
        }

        public async Task<IEnumerable<ClientModel>> GetClientsSortedByAmount(string searchedName, string sorted)
        {

            string extraOption = string.IsNullOrWhiteSpace(searchedName) 
                ? "" 
                : $"WHERE (FirstName + ' ' + LastName) LIKE '%{searchedName}%'";

            string query = "SELECT Id, CNP, (FirstName + ' ' + LastName) as FullName, Address, Total FROM clients C " +
                    "JOIN(SELECT ClientId, SUM(Amount) AS Total FROM accounts WHERE IsClosed = 0 GROUP BY ClientId) A ON C.Id = A.ClientId " +
                    $" {extraOption} ORDER BY Total {sorted}";

            return await ExecuteQuery<ClientModel>(query);
        }

        public async Task<IEnumerable<ClientModel>> GetClientsSortedByName(string searchedName, string sorted)
        {
            var extraOption = string.IsNullOrWhiteSpace(searchedName)
                ? ""
                : "WHERE (FirstName + ' ' + LastName) LIKE '%{searchedName}%'";

            string query = "SELECT Id, CNP, (FirstName + ' ' + LastName) as FullName, Address, Total FROM clients C " +
                    "JOIN(SELECT ClientId, SUM(Amount) AS Total FROM accounts WHERE IsClosed = 0 GROUP BY ClientId) A ON C.Id = A.ClientId " +
                    $" {extraOption} ORDER BY FullName { sorted }";

            return await ExecuteQuery<ClientModel>(query);
        }


        async Task<IEnumerable<T>> ExecuteQuery<T>(string query)
        {
            using var connection = new SqlConnection(_connectionString.Value);
            await connection.OpenAsync();
            var result = await connection.QueryAsync<T>(query);
            return result;
        }
    }
}
