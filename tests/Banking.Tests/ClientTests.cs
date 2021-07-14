using Banking.Core.ClientAggregate;
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
            AccountType accountType = new AccountType("Gold");

            _sut.CreateAccount(
            new Account(
                    "RO49AAAA1B31007593840000",
                    accountType,
                    Core.Shared.CurrencyType.RON
                ));


            _sut.Accounts.Count.Should().Be(1);
        }
    }
}
