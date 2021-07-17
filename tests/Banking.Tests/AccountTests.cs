using Banking.Core.AccountTypeFactory;
using Banking.Core.Entities;
using Banking.Core.Exceptions;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Banking.Tests
{
    public class AccountTests
    {
        private Account _sut;

        public AccountTests()
        {
            _sut = new Account(
                "1940724273941",
                "RO49AAAA1B31007593840000",
                new BasicAccountType().GetAccountType(),
                Shared.Enums.CurrencyType.RON
                );
        }

        [Fact]
        public void ShouldThrowInsufficientAmountExceptionWhenAmountIsToLow()
        {
            Func<decimal> func = () => _sut.Withdrawal(300);
            func.Should().Throw<InsufficientAmountException>();
        }

        [Theory]
        [InlineData(248)]
        [InlineData(1538)]
        [InlineData(23)]
        public void ShouldTakeTheCommissionWhenDeposit(decimal depositedValue)
        {
            var expectedValue = 0M;

            var commission = _sut.Deposit(depositedValue);

            expectedValue = depositedValue - commission;

            _sut.Amount.Should().Be(expectedValue);
        }

        [Fact]
        public void ShoulAddTransactionWhenDepositAnAmountOfMoney()
        {
            decimal trabsactionValue = 300M;
            decimal commision = _sut.Deposit(trabsactionValue);

            _sut.Transactions.Count().Should().Be(1);
            _sut.Transactions.First().Amount.Should().Be(trabsactionValue - commision);
        }

        [Theory]
        [InlineData(360, 200)]
        [InlineData(120, 110)]
        [InlineData(40, 25)]
        [InlineData(1024, 700)]
        public void Basic_ShouldTakeTheCommissionFromAccountAmountWhenWithdrawal(
            decimal depositedAmount, 
            decimal withdrawalAmount)
        {
            var commission = _sut.Deposit(depositedAmount);
            var expectedAmount = depositedAmount - commission;

            var withdrawalCommission = _sut.Withdrawal(withdrawalAmount);

            expectedAmount -= (withdrawalCommission + withdrawalAmount);

            _sut.Amount.Should().Be(expectedAmount);
        }

    }
}
