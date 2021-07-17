using Banking.Core.AccountTypeFactory;
using Banking.Core.Entities;
using Banking.Core.Interfaces;
using Banking.Shared.Enums;
using Banking.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core
{
    public class Facade : IFacade
    {
        private static string _encryptionKey = "everythingissecure";

        private readonly IClientRepository _clientRepository;
        private readonly AccountTypeProviderFactory _accountTypeFactory;

        private static Account _bankAccount;
        private static Account _cashAccount;

        public Facade(IClientRepository clientRepository, AccountTypeProviderFactory accountTypeFactory)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException($"clientRepository -> {nameof(Facade)}");
            _accountTypeFactory = accountTypeFactory ?? throw new ArgumentNullException($"accountTypeFactory -> {nameof(Facade)}");
            GenerateBankAccounts();
        }

        public void RegisterClient(string cnp, string pin, string firstName,
            string lastName, string address, AccountTypeEnum accountType, CurrencyType currencyType)
        {
            try
            {
                var encryptedPIN = EncryptionManager.Encrypt(pin, _encryptionKey);

                Client client = new(cnp, encryptedPIN, firstName, lastName, address);

                CreateAccount(cnp, accountType, currencyType);

                _clientRepository.Add(client);

            }
            catch
            {
                throw;
            }
        }

        public bool IdentifyClient(string cnp, string pin)
        {
            try
            {
                var client = _clientRepository.GetByCNP(cnp);

                if (client is null)
                    return false;

                var decryptedPIN = EncryptionManager.Decrypt(client.PIN, _encryptionKey);

                return pin == decryptedPIN ? true : false;
            }
            catch
            {
                throw;
            }
        }

        public void ChangeClientPIN(string cnp, string newPIN)
        {
            var client = _clientRepository.GetByCNP(cnp);
            var encryptedNewPIN = EncryptionManager.Encrypt(newPIN, _encryptionKey);
            client.ChangePIN(encryptedNewPIN);
        }

        public void Withdrawal(string cnp, string iban, decimal value)
        {
            try
            {
                Account account = GetAccountForClient(cnp, iban);

                decimal commission = account.Withdrawal(value);

                _cashAccount.Deposit(value);
                _bankAccount.Deposit(commission);
            }
            catch
            {
                throw;
            }
        }

        public void Deposit(string cnp, string iban, decimal value)
        {
            try
            {
                Account account = GetAccountForClient(cnp, iban);
                _cashAccount.Deposit(value);
                decimal commission = account.Deposit(value);
                _bankAccount.Deposit(commission);
            }
            catch
            {

                throw;
            }
        }

        public void CreateAccount(string cnp, AccountTypeEnum accountType, CurrencyType currencyType)
        {
            try
            {
                var _accountType = _accountTypeFactory.GetAccountTypeByType(accountType);
                var IBAN = IBANGenerator.Generate();

                var client = _clientRepository.GetByCNP(cnp);

                client.CreateAccount(
                    new Account(client.CNP, IBAN, _accountType, currencyType)
                    );

                //TODO: update client
            }
            catch
            {

                throw;
            }
        }


        void GenerateBankAccounts()
        {
            var IBAN = IBANGenerator.Generate();
            var accountType = _accountTypeFactory.GetAccountTypeByType(AccountTypeEnum.Gold);

            _bankAccount = new Account("0000000000000", IBAN, accountType, CurrencyType.RON);
            _cashAccount = new Account("0000000000000", IBAN, accountType, CurrencyType.RON);
        }
        Account GetAccountForClient(string cnp, string iban)
        {
            var client = _clientRepository.GetByCNP(cnp);
            return client.Accounts.FirstOrDefault(x => x.IBAN == iban);
        }
    }
}
