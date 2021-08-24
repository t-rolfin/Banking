using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.Wcf;
using Banking.Core;
using Banking.Core.AccountTypeFactory;
using Banking.Core.Interfaces;
using Banking.Core.Repositories;
using Banking.Core.Services;
using Banking.Infrastructure.Repositories;
using Banking.Infrastructure.Services;
using Banking.Shared.Helpers;
using Banking.Wcf.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Banking.Wcf
{
    public static class Bootstrapper
    {
        public static void RegisterDependencies()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();

            services.AddSingleton(
                new ExchangeRatesConfigurations(
                    ConfigurationManager.AppSettings["ExchangeRateUrl"].ToString(), "ThisAPIDoNotUseAPIKey")
                );

            services.AddTransient<IBankingService, BankingService>();
            services.AddTransient<IClientRepository, InMemoryClientRepository>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAccountRepository, InMemoryAccountRepository>();
            services.AddTransient<IExchangeRatesService, ExchangeRatesService>();
            services.AddSingleton<AccountTypeProviderFactory>();
            services.AddTransient<IFacade, Facade>();

            var providerFactory = new AutofacServiceProviderFactory();
            var builder = providerFactory.CreateBuilder(services);
            AutofacHostFactory.Container = builder.Build();
        }
    }
}