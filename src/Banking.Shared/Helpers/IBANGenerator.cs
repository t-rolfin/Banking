using SinKien.IBAN4Net;
using System;

namespace Banking.Shared.Helpers
{
    public static class IBANGenerator
    {
        public static string Generate()
        {
            var random = new Random();

            Iban iban = new IbanBuilder()
                .CountryCode(CountryCode.GetCountryCode("RO"))
                .BankCode("dsfa".ToUpper())
                .AccountNumberPrefix(random.Next(100, 999).ToString())
                .AccountNumber($"200014539947{random.Next(1000, 9999)}")
                .Build(true, false);

            return iban.ToString();
        }
    }
}
