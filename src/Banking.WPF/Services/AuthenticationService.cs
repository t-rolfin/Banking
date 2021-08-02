using Banking.Core;
using Banking.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.WPF.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IFacade _facade;

        public AuthenticationService(IFacade facade)
        {
            _facade = facade;
        }

        public async Task<Client> LogIn(string cnp, string pin)
        {
            var client = await _facade.IdentifyClient(cnp, pin);

            if(client is null)
            {
                return null;
            }
            else
            {
                return new Client() { FullName = client.GetFullName(), Id = client.Id };
            }
        }
    }
}
