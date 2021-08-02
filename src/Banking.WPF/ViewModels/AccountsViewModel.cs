using Banking.WPF.Models;
using Banking.WPF.Services;
using Banking.WPF.Stores;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Banking.WPF.ViewModels
{
    public class AccountsViewModel : ViewModelBase
    {
        private readonly IAccountService _accountService;
        private ObservableCollection<Account> _accounts;
        public ObservableCollection<Account> Accounts {
            get => _accounts; 
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        public ICommand LoginCommand { get; }

        public AccountsViewModel(
            AccountsStore accountsStore, 
            INavigationService acountNavigationService, 
            IAccountService accountService,
            ClientStore clientStore)
        {
            _accountService = accountService;

            Task.Run(async () =>
           {
               Accounts = await _accountService.GetClientAccounts(clientStore.CurrentClient.Id);
           });
        }
    }
}
