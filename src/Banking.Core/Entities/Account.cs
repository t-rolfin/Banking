using System.Collections.Generic;
using Banking.Core.Exceptions;
using Banking.Shared.Enums;
using Ardalis.GuardClauses;
using Banking.Core.Enums;
using System;

namespace Banking.Core.Entities
{
    public class Account
    {
        List<Transaction> _transactions = new();

        public Account(string clientCNP, string iban, AccountType accountType, CurrencyType currencyType)
        {
            Id = Guid.NewGuid();
            ClientCNP = Guard.Against.InvalidFormat(clientCNP, nameof(clientCNP), "[0-9]{13}", "The length of CNP must be 13.");
            IBAN = Guard.Against.InvalidInput(iban, nameof(iban), (x) => 
            {
                if (x.Length != 24)
                    return false;
                else
                    return true;
            }, "Invalid IBAN! If this error occur contact support team.");
            AccountType = Guard.Against.Null(accountType, nameof(AccountType), "An account type must be provided.");
            CurrencyType = Guard.Against.Null(currencyType, nameof(currencyType), "A currency type wasn't selected.");
        }

        public Guid Id { get; }
        public string ClientCNP { get; init; }
        public string IBAN { get; set; }
        public AccountType AccountType { get; init; }
        public CurrencyType CurrencyType { get; init; }
        public decimal Amount { get; protected set; }
        public bool IsClosed { get; protected set; }

        public IReadOnlyList<Transaction> Transactions
            => _transactions.AsReadOnly();


        public decimal Withdrawal(decimal withdrawalValue)
        {
            if(withdrawalValue > this.Amount) throw new InsufficientAmountException();

            if (this.AccountType.HasCommisions)
            {
                var commissionValue = ApplyCommission(OperationType.Withdrawal, withdrawalValue);

                this.Amount -= (withdrawalValue + commissionValue);

                _transactions.Add(
                        new Transaction(
                                withdrawalValue + commissionValue,
                                this,
                                null,
                                OperationType.Withdrawal,
                                this.CurrencyType
                            )
                    );

                return commissionValue;
            }
            else
            {
                this.Amount -= withdrawalValue;
                return 0;
            }
        }
        
        public decimal Deposit(decimal depositedValue)
        {
            if (this.AccountType.HasCommisions)
            {
                var commissionValue = ApplyCommission(OperationType.Withdrawal, depositedValue);

                Amount += (depositedValue - commissionValue);

                _transactions.Add(
                        new Transaction(
                            depositedValue - commissionValue,
                            null,
                            this,
                            OperationType.Deposit,
                            this.CurrencyType)
                    );

                return commissionValue;
            }
            else
            {
                this.Amount += depositedValue;
                return 0;
            }
        }

        public void CloseAccount()
        {
            this.IsClosed = true;
        }

        decimal ApplyCommission(OperationType operationType, decimal value)
        {
            var commission = AccountType.Operations
                .Find(x => x.OperationType == operationType)
                .Commission;

            return value * (decimal)(commission.Percent / 100) + (decimal)commission.FixedValue;

        }
    }
}
