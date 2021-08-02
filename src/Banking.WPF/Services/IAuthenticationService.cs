using Banking.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.WPF.Services
{
    public interface IAuthenticationService
    {
        Task<Client> LogIn(string cnp, string pin);
    }
}
