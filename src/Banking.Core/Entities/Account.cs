using System.Collections.Generic;
using Banking.Core.Exceptions;
using Banking.Shared.Enums;
using Ardalis.GuardClauses;
using System;

namespace Banking.Core.Entities
{
    public class Account
    {
        List<Transaction> _transactions = new();

        protected Account() { }

        public Account(Guid clientId, string iban, AccountType accountType, CurrencyType currencyType)
        {
            Id = Guid.NewGuid();
            ClientId = Guard.Against.Null(clientId, nameof(clientId));
            IBAN = Guard.Against.InvalidInput(iban, nameof(iban), (x) => 
            {
                if (x.Length != 24)
                    return false;
                else
                    return true;
            }, "Invalid IBAN! If this error occur contact support team.");
            AccountType = Guard.Against.Null(accountType, nameof(AccountType), "An account type must be provided.");
            CurrencyType = Guard.Against.Null(currencyType, nameof(currencyType), "A currency type wasn't selected.");
            IsNew = true;
        }

        public Guid Id { get; }
        public Guid ClientId { get; init; }
        public string IBAN { get; set; }
        public virtual AccountType AccountType { get; protected set; }
        public virtual CurrencyType CurrencyType { get; init; }
        public decimal Amount { get; set; }
        public bool IsClosed { get; protected set; }
        public bool IsNew { get; protected set; }

        public virtual IReadOnlyList<Transaction> Transactions
            => _transactions.AsReadOnly();

        public void AddTransaction(Transaction transaction)
        {
            if (transaction is null || transaction == default)
                throw new ArgumentNullException();

            _transactions.Add(transaction);
        }

        public void SetAccountType(AccountType accountType)
        {
            if (accountType is null || accountType is default(AccountType))
                throw new ArgumentNullException();

            if (this.AccountType is null || this.AccountType is default(AccountType))
                this.AccountType = accountType;
            else
                throw new AccountTypeCanNotBeSetException();
        }

        public decimal Withdrawal(decimal withdrawalValue)
        {
            if(withdrawalValue > this.Amount) 
                throw new InsufficientAmountException("You don't have the required amount of money!");

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

                _transactions.Add(
                    new Transaction(
                            withdrawalValue,
                            this,
                            null,
                            OperationType.Withdrawal,
                            this.CurrencyType
                        )
                );

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

                _transactions.Add(
                    new Transaction(
                            depositedValue,
                            null,
                            this,
                            OperationType.Deposit,
                            this.CurrencyType
                        )
                );

                return 0;
            }
        }

        public void CloseAccount()
        {
            if (this.Amount > 0 || this.Amount < 0)
                throw new AccountCanNotBeClosedException(
                        "The account amount must be 0 to be closed."
                    );

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
