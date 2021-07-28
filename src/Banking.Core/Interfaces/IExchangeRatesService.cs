using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.Interfaces
{
    public interface IExchangeRatesService
    {
        Task<decimal> ConvertMoneyAsync(CurrencyType from, CurrencyType to, decimal value);
    }
}
