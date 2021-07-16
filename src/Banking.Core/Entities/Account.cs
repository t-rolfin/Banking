using Banking.Core.Enums;
using Banking.Core.Exceptions;
using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.Entities
{
    public class Account
    {
        List<Transaction> _transactions = new List<Transaction>();

        public Account(string clientCNP, string iban, AccountType accountType, CurrencyType currencyType)
        {
            Id = Guid.NewGuid();
            IBAN = iban;
            AccountType = accountType;
            CurrencyType = currencyType;
            ClientCNP = clientCNP;
        }

        public Guid Id { get; }
        public string ClientCNP { get; }
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
