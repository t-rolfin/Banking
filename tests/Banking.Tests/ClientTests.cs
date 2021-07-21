using Banking.Core.Entities;
using Banking.Shared.Enums;
using FluentAssertions;
using System;
using Xunit;

namespace Banking.Tests
{
    public class ClientTests
    {
        private readonly Client _sut;

        public ClientTests()
        {
            _sut = new Client("1928489323845", "2480", "Jhon", "Elton", "Unknown");
        }

        [Fact]
        public void ShouldCreateAnAccount()
        {
            AccountType accountType = new AccountType("Gold", 2);

            _sut.CreateAccount(
            new Account(
                    Guid.NewGuid(),
                    "RO49AAAA1B31007593840000",
                    accountType,
                    CurrencyType.RON
                ));


            _sut.Accounts.Count.Should().Be(1);
        }

        [Fact]
        public void ShouldChangeThePINWhenChangePINIsCalled()
        {
            var newPIN = "2482";

            _sut.ChangePIN(newPIN);

            _sut.PIN.Should().Be(newPIN);
        }

        [Fact]
        public  void ShouldSetAccoutIsClosedToTrueWheCallCloseAccount()
        {
            AccountType accountType = new AccountType("Gold", 2);

            _sut.CreateAccount(
            new Account(
                    Guid.NewGuid(),
                    "RO49AAAA1B31007593840000",
                    accountType,
                    CurrencyType.RON
                ));

            _sut.Accounts[0].CloseAccount();

            _sut.Accounts[0].IsClosed.Should().BeTrue();
        }
    }
}
