using Ardalis.GuardClauses;
using Banking.Core.Exceptions;
using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.Entities
{
    public class Client
    {
        List<Account> _accounts = new List<Account>();

        protected Client() { }

        public Client(string cnp, string pin, string firstName, string lastName, string address)
        {
            Id = Guid.NewGuid();
            CNP = Guard.Against.InvalidFormat(cnp, nameof(cnp), "[0-9]{13}", "The length of CNP must be 13.");
            PIN = Guard.Against.NullOrWhiteSpace(pin, nameof(pin), "The PIN can't be empty!");
            FirstName = Guard.Against.NullOrWhiteSpace(firstName, nameof(firstName), "The field FirstName can't be empty!");
            LastName = Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName), "The field LastName can't be empty!");
            Address = address;
            HasNewAccount = true;
        }

        public Client(string cnp, string pin, string firstName, string lastName, string address, Account account)
            : this(cnp, pin, firstName, lastName, address)
        {
            this._accounts.Add(account);
        }


        public Guid Id { get; set; }
        public string CNP { get; init; }
        public string PIN { get; set; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Address { get; init; }
        public bool HasNewAccount { get; protected set; }
        public virtual IReadOnlyList<Account> Accounts => _accounts.AsReadOnly();


        public void CreateAccount(Account newAccount)
        {
            _accounts.Add(newAccount);
            HasNewAccount = true;
        }

        public void CloseAccount(Guid accountId)
        {
            var account = _accounts.Find(x => x.Id == accountId)
                ?? throw new AccountNotFoundException("There is no account with specified ID.");

            account.CloseAccount();
        }

        public void ChangePIN(string newPIN)
        {
            PIN = Guard.Against.NullOrWhiteSpace(newPIN, nameof(newPIN), "The PIN can't be empty!");
            this.PIN = newPIN;
        }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
