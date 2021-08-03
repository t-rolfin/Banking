using Banking.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.WPF.Stores
{
    public class ClientStore
    {
        private Client _currentClient;
        public Client CurrentClient {
            get => _currentClient;
            set {
                _currentClient = value;
                CurrentClientChanged?.Invoke();
            } 
        }

        public bool IsLoggedIn => CurrentClient != null;

        public event Action CurrentClientChanged;

        public void LogOut()
        {
            CurrentClient = null;
        }
    }
}
