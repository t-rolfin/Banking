using Banking.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.WPF.Stores
{
    public class AccountsStore
    {
        private ObservableCollection<Account> _accounts;
        public ObservableCollection<Account> Accounts {
            get => _accounts;
            set {
                _accounts = value;
            } 
        }
    }
}
