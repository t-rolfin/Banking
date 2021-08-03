using Banking.Core.AccountTypeFactory;
using Banking.Core.Entities;
using Banking.Core.Exceptions;
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
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountService _accountService;
        private readonly AccountTypeProviderFactory _accountTypeFactory;

        private static Account _bankAccount;
        private static Account _cashAccount;

        public Facade(
            IClientRepository 
            clientRepository, 
            AccountTypeProviderFactory accountTypeFactory, 
            IAccountRepository accountRepository, 
            IAccountService accountService)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException($"clientRepository -> {nameof(Facade)}");
            _accountTypeFactory = accountTypeFactory ?? throw new ArgumentNullException($"accountTypeFactory -> {nameof(Facade)}");
            _accountRepository = accountRepository ?? throw new ArgumentNullException($"accountRepository -> {nameof(Facade)}");
            _accountService = accountService ?? throw new ArgumentNullException($"accountService -> {nameof(Facade)}");
            GenerateBankAccounts();
        }

        public async Task<Client> RegisterClient(string cnp, string pin, string firstName,
            string lastName, string address, AccountTypeEnum accountType, CurrencyType currencyType,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (await _clientRepository.GetByCNPAsync(cnp) is not null)
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
                var client = await _clientRepository.GetByCNPAsync(cnp);

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
            var client = await _clientRepository.GetByCNPAsync(cnp);
            var encryptedNewPIN = EncryptionManager.Encrypt(newPIN, _encryptionKey);
            client.ChangePIN(encryptedNewPIN);

            await _clientRepository.UpdateAsync(client, cancellationToken);
        }

        public async Task Transfer(Guid accountId, string destinationIban, decimal value, CancellationToken cancellationToken)
        {
            var sourceAccount = await _accountRepository.GetById(accountId);
            var destinationAccount = await _accountRepository.GetByIBAN(destinationIban);

            var commission = await _accountService.Transfer(sourceAccount, destinationAccount, value);
            _bankAccount.Deposit(commission);

            await _accountRepository.UpdateAccountList(cancellationToken, sourceAccount, destinationAccount);
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

                var client = await _clientRepository.GetByCNPAsync(cnp);

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

        async Task<Account> GetAccountForClient(string cnp, string iban)
        {
            var client = await _clientRepository.GetByCNPAsync(cnp);
            return client.Accounts.FirstOrDefault(x => x.IBAN == iban);
        }

        public async Task<Account> CreateAccountFor(Guid clientId, AccountTypeEnum accountTypeEnum, CurrencyType currencyType, CancellationToken cancellationToken)
        {
            Client client = await _clientRepository.GetByIdAsync(clientId);

            if (client is not null)
            {
                string iban = IBANGenerator.Generate();
                AccountType accountType = _accountTypeFactory.GetAccountTypeByType(accountTypeEnum);
                var account = new Account(clientId, iban, accountType, currencyType);
                client.CreateAccount(account);

                var response = await _clientRepository.UpdateAsync(client, cancellationToken);

                return response ? account : null;
            }
            else
            {
                throw new ClientNotFoundException();
            }

        }
        public async Task<bool> CloseAccount(Guid clientId, Guid accountId, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);

            if (client is not null)
            {
                try
                {
                    client.CloseAccount(accountId);
                    await _clientRepository.UpdateAsync(client, cancellationToken);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }


        void GenerateBankAccounts()
        {
            var IBAN = IBANGenerator.Generate();
            var accountType = _accountTypeFactory.GetAccountTypeByType(AccountTypeEnum.Gold);

            _bankAccount = new Account(Guid.NewGuid(), IBAN, accountType, CurrencyType.RON);
            _cashAccount = new Account(Guid.NewGuid(), IBAN, accountType, CurrencyType.RON);
        }
    }
}
