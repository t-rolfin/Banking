using Banking.WPF.Commands;
using Banking.WPF.Services;
using Banking.WPF.Stores;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Banking.WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _cnp;
        public string CNP
        {
            get
            {
                return _cnp;
            }
            set
            {
                _cnp = value;
                OnPropertyChanged(nameof(CNP));
            }
        }

        private string _pin;
        public string PIN
        {
            get
            {
                return _pin;
            }
            set
            {
                _pin = value;
                OnPropertyChanged(nameof(PIN));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(ClientStore accountStore, INavigationService loginNavigationService, IAuthenticationService _authenticationService)
        {
            LoginCommand = new LoginCommand(this, accountStore, loginNavigationService, _authenticationService);
        }
    }
}
