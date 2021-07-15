using AutoFixture;
using Banking.Core.AccountTypeFactory;
using Banking.Core.Entities;
using Banking.Core.Exceptions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Banking.Tests
{
    public class AccountTests
    {

        private Account _sut;
        private Account _sutGold;

        public AccountTests()
        {
            var fixture = new Fixture();

            _sut = fixture.Build<Account>()
                .With(x => x.AccountType, new BasicAccountType().GetAccountType())
                .Create();

            _sutGold = fixture.Build<Account>()
                .With(x => x.AccountType, new GoldAccountType().GetAccountType())
                .Create();
        }

        [Fact]
        public void Basic_ShouldThrowInsufficientAmountExceptionWhenAmountIsToLow()
        {
            Func<decimal> func = () => _sut.Withdrawal(300);
            func.Should().Throw<InsufficientAmountException>();
        }

        [Fact]
        public void Basic_ShouldTakeTheCommissionWhenDeposit()
        {
            decimal expect = 297;

            _sut.Deposit(300);

            _sut.Amount.Should().Be(expect);
        }

        [Fact]
        public void Basic_ShoulAddTransactionWheDepositAnAmountOfMoney()
        {
            _sut.Deposit(300);

            _sut.Transactions.Count().Should().Be(1);
        }

        [Fact]
        public void Gold_ShouldNotTakeTheCommissionWhenDeposit()
        {
            int expect = 789;

            _sutGold.Deposit(789);

            _sutGold.Amount.Should().Be(expect);
        }

    }
}
