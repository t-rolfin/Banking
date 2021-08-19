using Autofac;
using Autofac.Integration.Wcf;
using Banking.Wcf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Wcf
{
    public static class Bootstrapper
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<BankingService>()
                .As<IBankingService>();

            AutofacHostFactory.Container = builder.Build();
        }
    }
}