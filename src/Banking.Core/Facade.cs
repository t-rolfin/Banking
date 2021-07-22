using Banking.Core.AccountTypeFactory;
using Banking.Core.Entities;
using Banking.Core.Interfaces;
using Banking.Shared.Enums;
using Banking.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public async Task<Client> RegisterClient(string cnp, string pin, string firstName,
            string lastName, string address, AccountTypeEnum accountType, CurrencyType currencyType,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (await _clientRepository.GetClientByCNPAsync(cnp) is not null)
                    return null;

                var _accountType = _accountTypeFactory.GetAccountTypeByType(accountType);
                var IBAN = IBANGenerator.Generate();

                var encryptedPIN = EncryptionManager.Encrypt(pin, _encryptionKey);

                Client client = new(cnp, encryptedPIN, firstName, lastName, address);

                client.CreateAccount(
                        new Account(client.Id, IBAN, _accountType, currencyType)
                    );

                await _clientRepository.CreateAsync(client, cancellationToken);

                return client;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<Client> IdentifyClient(string cnp, string pin)
        {
            try
            {
                var client = await _clientRepository.GetClientByCNPAsync(cnp);

                if (client is null)
                    return null;

                var decryptedPIN = EncryptionManager.Decrypt(client.PIN, _encryptionKey);

                return pin == decryptedPIN ? client : null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task ChangeClientPIN(string cnp, string newPIN, CancellationToken cancellationToken = default)
        {
            var client = await _clientRepository.GetClientByCNPAsync(cnp);
            var encryptedNewPIN = EncryptionManager.Encrypt(newPIN, _encryptionKey);
            client.ChangePIN(encryptedNewPIN);

            await _clientRepository.UpdateAsync(client, cancellationToken);
        }

        public async Task Withdrawal(Guid clientId, Guid accountId, decimal value, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(clientId);
                var account = client?.Accounts.First(x => x.Id == accountId);

                decimal commission = account.Withdrawal(value);

                _cashAccount.Deposit(value);
                _bankAccount.Deposit(commission);

                await _clientRepository.UpdateAsync(client, cancellationToken);
            }
            catch
            {
                throw;
            }
        }

        public async Task Deposit(Guid clientId, Guid accountId, decimal value, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(clientId);
                var account = client.Accounts.First(x => x.Id == accountId);
                _cashAccount.Deposit(value);
                decimal commission = account.Deposit(value);
                _bankAccount.Deposit(commission);

                await _clientRepository.UpdateAsync(client, cancellationToken);
            }
            catch
            {
                throw;
            }
        }

        public async Task CreateAccount(string cnp, AccountTypeEnum accountType, CurrencyType currencyType, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var _accountType = _accountTypeFactory.GetAccountTypeByType(accountType);
                var IBAN = IBANGenerator.Generate();

                var client = await _clientRepository.GetClientByCNPAsync(cnp);

                client.CreateAccount(
                    new Account(client.Id, IBAN, _accountType, currencyType)
                    );

                await _clientRepository.UpdateAsync(client, cancellationToken);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IReadOnlyList<Account>> GetUserAccounts(Guid id)
        {
            return await _clientRepository.GetClientAccountsById(id);
        }


        void GenerateBankAccounts()
        {
            var IBAN = IBANGenerator.Generate();
            var accountType = _accountTypeFactory.GetAccountTypeByType(AccountTypeEnum.Gold);

            _bankAccount = new Account(Guid.NewGuid(), IBAN, accountType, CurrencyType.RON);
            _cashAccount = new Account(Guid.NewGuid(), IBAN, accountType, CurrencyType.RON);
        }
        async Task<Account> GetAccountForClient(string cnp, string iban)
        {
            var client = await _clientRepository.GetClientByCNPAsync(cnp);
            return client.Accounts.FirstOrDefault(x => x.IBAN == iban);
        }
    }
}
