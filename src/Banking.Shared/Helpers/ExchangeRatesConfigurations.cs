using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Helpers
{
    public class ExchangeRatesConfigurations
    {
        public ExchangeRatesConfigurations(string url, string accessKey)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException($"'{nameof(url)}' cannot be null or whitespace.", nameof(url));
            }

            if (string.IsNullOrWhiteSpace(accessKey))
            {
                throw new ArgumentException($"'{nameof(accessKey)}' cannot be null or whitespace.", nameof(accessKey));
            }

            Url = url;
            AccessKey = accessKey;
        }

        public string Url { get; }
        public string AccessKey { get; }
    }
}
