using Banking.Core.Exceptions;
using Banking.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.ClientAggregate
{
    public class Client : IRootAggregate
    {
        List<Account> _accounts = new List<Account>();

        protected Client() { }
        protected Client(string cnp, string firstName, string lastName, string address)
        {
            CNP = cnp;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
        }

        // Este folosit ca si ID
        public string CNP { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Address { get; init; }
        public IReadOnlyList<Account> Accounts => _accounts.AsReadOnly();

        public void CreateAccount(Account newAccount)
        {
            //TODO: validare date
            _accounts.Add(newAccount);
        }

        public void CloseAccount(Guid accountId) 
            => _accounts.Remove(CheckIfAccountExist(accountId));

        public void ChangePINFor(Guid accountId, string newPIN)
        {
            //TODO: varificare PIN

            var requiredAccount = CheckIfAccountExist(accountId);
            requiredAccount.ChangePIN(newPIN);
        }

        Account CheckIfAccountExist(Guid accountId)
            => _accounts.Find(x => x.Id == accountId) 
            ?? throw new AccountNotFoundException("There is no account with specified ID.");


    }
}
