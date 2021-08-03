using Banking.WPF.Models;
using Banking.WPF.Services;
using Banking.WPF.Stores;
using Banking.WPF.ViewModels;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.WPF.Commands
{
    public class LoginCommand : AsyncCommandBase
    {

        private readonly LoginViewModel _loginViewModel;
        private readonly ClientStore _clientStore;
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authenticationService;

        public LoginCommand(
            LoginViewModel loginViewModel, 
            ClientStore clientStore, 
            INavigationService navigationService,
            IAuthenticationService authenticationService)
        {
            _loginViewModel = loginViewModel;
            _clientStore = clientStore;
            _navigationService = navigationService;
            _authenticationService = authenticationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            Client client = await _authenticationService.LogIn(_loginViewModel.CNP, _loginViewModel.PIN);

            _clientStore.CurrentClient = client;

            _navigationService.Navigate();
        }
    }
}
