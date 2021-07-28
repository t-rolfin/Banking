using Banking.Core.Interfaces;
using Banking.Shared.Enums;
using Banking.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;

namespace Banking.Infrastructure.Services
{
    public class ExchangeRatesService : IExchangeRatesService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ExchangeRatesConfigurations _apiConfigs;

        public ExchangeRatesService(IHttpClientFactory httpClient, ExchangeRatesConfigurations apiConfigs)
        {
            _httpClient = httpClient;
            _apiConfigs = apiConfigs;
        }

        public async Task<decimal> ConvertMoneyAsync(CurrencyType from, CurrencyType to, decimal value)
        {
            if (from == CurrencyType.RON)
                return value;

            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(_apiConfigs.Url);

            var response = await client.GetAsync($"?amount={value}&from={from}&to={to}");

            var stringResponse = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<ExchangeRatesResponse>(stringResponse);

            return to == CurrencyType.RON ? deserializedResponse.rates.RON : deserializedResponse.rates.EUR;
        }
    }

    public class Rates
    {
        public decimal RON { get; set; }
        public decimal EUR { get; set; }
    }

    public class ExchangeRatesResponse
    {
        public double amount { get; set; }
        public string @base { get; set; }
        public string date { get; set; }
        public Rates rates { get; set; }
    }
}
