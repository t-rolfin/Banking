using Banking.Core;
using Banking.Core.AccountTypeFactory;
using Banking.Core.Interfaces;
using Banking.Core.Services;
using Banking.Infrastructure;
using Banking.Infrastructure.Repositories;
using Banking.Infrastructure.Services;
using Banking.Shared.Helpers;
using Banking.WPF.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using System.Windows;

namespace Banking.WPF
{
    public partial class App : Application
    {
        //private ServiceProvider _serviceProvider;
        //private readonly IConfiguration Configuration;

        private IConfiguration AddConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
#if DEBUG
            builder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
#else
            builder.AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
#endif

            return builder.Build();
        }

        public App()
        {
            //Configuration = AddConfiguration();
            //ServiceCollection services = new ServiceCollection();
            //ConfigureServices(services);
            //_serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow();
            window.DataContext = new MainViewModel();
            window.Show();
            base.OnStartup(e);
        }

        //private void ConfigureServices(ServiceCollection services)
        //{
        //    services.AddSingleton(AddConfiguration());

        //    services.AddSingleton(x => {
        //        var connectionString = Configuration?.GetSection("ConnectionString").Value;
        //        return new ConnectionString(connectionString);
        //    });

        //    services.AddSingleton(x => {
        //        var exchangeConfigs = Configuration.GetSection("ExchangeRates").GetChildren();
        //        return new ExchangeRatesConfigurations(
        //                exchangeConfigs.First(x => x.Key == "Url").Value,
        //                exchangeConfigs.First(x => x.Key == "AccessKey").Value
        //            );
        //    });

        //    services.AddDbContext<ClientContext>();
        //    services.AddTransient<ClientContext>();

        //    services.AddHttpClient();

        //    //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        //    //    .AddCookie(x =>
        //    //    {
        //    //        x.LoginPath = "/Account/LogIn";
        //    //    });

        //    //services.AddAuthorization(o =>
        //    //{
        //    //    o.AddPolicy("Operator", policy => policy.Requirements.Add(new HasRoleRequirement("Operator")));
        //    //});

        //    services.AddTransient<IFacade, Facade>();
        //    services.AddTransient<IClientRepository, ClientRepository>();
        //    services.AddTransient<IAccountService, AccountService>();
        //    services.AddTransient<IAccountRepository, AccountRepository>();
        //    services.AddTransient<IQueryRepository, ClientQueryRepository>();
        //    services.AddTransient<IExchangeRatesService, ExchangeRatesService>();
        //    services.AddTransient<IFileExportService, FileExportService>();

        //    services.AddSingleton<AccountTypeProviderFactory>();
        //    services.AddSingleton<IOperatorService, OperatorService>();
        //    //services.AddSingleton<IAuthorizationHandler, HasRoleHandler>();
        //    services.AddSingleton<MainWindow>();
        //}
    }
}
