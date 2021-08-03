using Banking.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.WPF.Services
{
    public interface IAccountService
    {
        Task<ObservableCollection<Account>> GetClientAccounts(Guid ClientId);
        Task<bool> Withdrawal(Guid clientId, Guid accountId, decimal value);
        Task<bool> Deposit(Guid clientId, Guid accountId, decimal value);
    }
}
