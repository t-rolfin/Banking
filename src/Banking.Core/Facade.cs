using Banking.Core.AccountTypeFactory;
using Banking.Core.Entities;
using Banking.Core.Exceptions;
using Banking.Core.Interfaces;
using Banking.Shared.Enums;
using Banking.Shared.Helpers;
using Rolfin.Result;
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

        public async Task<Result<Client>> RegisterClient(string cnp, string pin, string firstName,
            string lastName, string address, AccountTypeEnum accountType, CurrencyType currencyType,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (await _clientRepository.GetByCNPAsync(cnp) is not null)
                    return Result<Client>.Invalid()
                        .With("An account with this CNP already exists.");

                var _accountType = _accountTypeFactory.GetAccountTypeByType(accountType);
                var IBAN = IBANGenerator.Generate();

                var encryptedPIN = EncryptionManager.Encrypt(pin, _encryptionKey);

                Client client = new(cnp, encryptedPIN, firstName, lastName, address);

                client.CreateAccount(
                        new Account(client.Id, IBAN, _accountType, currencyType)
                    );

                await _clientRepository.CreateAsync(client, cancellationToken);

                return Result<Client>.Success(client);
            }
            catch (Exception ex)
            {
                return Result<Client>.Invalid()
                    .With(ex.Message);
            }
        }

        public async Task<Client> IdentifyClient(string cnp, string pin)
        {
            try
            {
                var client = await _clientRepository.GetByCNPAsync(cnp);

                if (client is null)
                    return Result<Client>.Invalid()
                        .With("An account with specified date does not exists.");

                var decryptedPIN = EncryptionManager.Decrypt(client.PIN, _encryptionKey);

                return pin == decryptedPIN
                    ? Result<Client>.Success(client)
                    : Result<Client>.Invalid().With("PIN Invalid.");
            }
            catch (Exception ex)
            {
                return Result<Client>.Invalid().With(ex.Message);
            }
        }

        public async Task<Result<bool>> ChangeClientPIN(string cnp, string newPIN, CancellationToken cancellationToken = default)
        {
            var client = await _clientRepository.GetByCNPAsync(cnp);

            if (client is null)
                return Result<bool>.Invalid()
                    .With("An user with CNP can not be found!");

            var encryptedNewPIN = EncryptionManager.Encrypt(newPIN, _encryptionKey);
            client.ChangePIN(encryptedNewPIN);

            var respose = await _clientRepository.UpdateAsync(client, cancellationToken);

            return respose 
                ? Result<bool>.Success() 
                : Result<bool>.Invalid().With("Somthing went wrong, please try again!");
        }

        public async Task<Result<bool>> Transfer(Guid accountId, string destinationIban, decimal value, CancellationToken cancellationToken)
        {
            try
            {
                var sourceAccount = await _accountRepository.GetById(accountId);
                var destinationAccount = await _accountRepository.GetByIBAN(destinationIban);
                var commission = await _accountService.Transfer(sourceAccount, destinationAccount, value);
                _bankAccount.Deposit(commission);
                await _accountRepository.UpdateAccountList(cancellationToken, sourceAccount, destinationAccount);

                return Result<bool>.Success();
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }

        public async Task<Result<bool>> Withdrawal(Guid clientId, Guid accountId, decimal value, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(clientId);
                var account = client?.Accounts.First(x => x.Id == accountId);
                decimal commission = account.Withdrawal(value);
                _cashAccount.Deposit(value);
                _bankAccount.Deposit(commission);
                await _clientRepository.UpdateAsync(client, cancellationToken);

                return Result<bool>.Success()
                    .With($"You Withdrawal with success {value} {account.CurrencyType} from account.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
            }
        }

        public async Task<Result<bool>> Deposit(Guid clientId, Guid accountId, decimal value, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(clientId);
                var account = client.Accounts.First(x => x.Id == accountId);
                _cashAccount.Deposit(value);
                decimal commission = account.Deposit(value);
                _bankAccount.Deposit(commission);
                await _clientRepository.UpdateAsync(client, cancellationToken);

                return Result<bool>.Success()
                    .With($"You deposited with success {value} {account.CurrencyType} into account.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid().With(ex.Message);
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
        
        public async Task<Result<bool>> CloseAccount(Guid clientId, Guid accountId, CancellationToken cancellationToken)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(clientId);
                client.CloseAccount(accountId);
                await _clientRepository.UpdateAsync(client, cancellationToken);
                return Result<bool>.Success()
                    .With("Your account was successfully closed!");
            }
            catch (Exception ex)
            {
                return Result<bool>.Invalid()
                    .With(ex.Message);
            }
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
