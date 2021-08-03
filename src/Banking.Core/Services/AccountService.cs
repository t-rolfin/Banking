using Banking.Core.Entities;
using Banking.Core.Interfaces;
using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banking.Core.Exceptions;

namespace Banking.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IExchangeRatesService _exchangeService;

        public AccountService(IExchangeRatesService ratesService)
        {
            _exchangeService = ratesService;
        }

        public async Task<decimal> Transfer(Account sourceAccount, Account destinationAccount, decimal transferValue)
        {

            if (sourceAccount.Amount < transferValue)
                throw new InsufficientAmountException("You can't transfer more money then you have!");

            var commission = ApplyCommission(sourceAccount, OperationType.Transfer, transferValue);

            sourceAccount.Amount -= (transferValue + commission);

            sourceAccount.AddTransaction(
                    new Transaction(transferValue, sourceAccount, destinationAccount, OperationType.Transfer, sourceAccount.CurrencyType)
                );

            var convertedMoney = await _exchangeService.ConvertMoneyAsync(sourceAccount.CurrencyType, destinationAccount.CurrencyType, transferValue);

            destinationAccount.Amount += sourceAccount.CurrencyType == destinationAccount.CurrencyType
                ? transferValue
                : convertedMoney;

            destinationAccount.AddTransaction(
                    new Transaction(
                    sourceAccount.CurrencyType == destinationAccount.CurrencyType
                        ? transferValue
                        : convertedMoney,
                    sourceAccount, destinationAccount, OperationType.Transfer, destinationAccount.CurrencyType)
                );

            return commission;
        }

        decimal ApplyCommission(Account account, OperationType operationType, decimal value)
        {
            var commission = account.AccountType.Operations
                .Find(x => x.OperationType == operationType)
                .Commission;

            return value * (decimal)(commission.Percent / 100) + (decimal)commission.FixedValue;
        }
    }
}
