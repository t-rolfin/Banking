using Banking.Core.Enums;
using Banking.Core.Shared;
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

        public Account(string iban, AccountType accountType, CurrencyType currencyType)
        {
            Id = Guid.NewGuid();
            IBAN = iban;
            AccountType = accountType;
            CurrencyType = currencyType;
        }

        public Guid Id { get; set; }
        public string IBAN { get; set; }
        public AccountType AccountType { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public decimal Amount { get; set; }
        public bool IsClosed { get; protected set; }

        public IReadOnlyList<Transaction> Transactions
            => _transactions.AsReadOnly();


        public decimal Withdrawal(decimal withdrawalValue)
        {
            if (this.AccountType.HasCommisions)
            {
                var commissions = GetCommissionValuesByOperationType(OperationType.Withdrawal);

                var commissionValue = withdrawalValue * (commissions.percent / 100) + commissions.fixedValue;

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
                return 0;
            }
        }
        
        public decimal Deposit(decimal depositedValue)
        {
            if (this.AccountType.HasCommisions)
            {
                var commissions = GetCommissionValuesByOperationType(OperationType.Deposit);

                var commissionValue = depositedValue * (commissions.percent / 100) + commissions.fixedValue;

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
                return 0;
            }
        }


        public void CloseAccount()
        {
            this.IsClosed = true;
        }

        (decimal percent, decimal fixedValue) GetCommissionValuesByOperationType(OperationType operationType)
        {
            var commission = AccountType.Operations
                .Find(x => x.OperationType == operationType)
                .Commission;

            return ((decimal)commission.Percent, (decimal)commission.Fixed);
        }

    }
}
