using Banking.Core;
using Banking.Core.AccountTypeFactory;
using Banking.Core.Interfaces;
using Banking.Core.Services;
using Banking.Infrastructure;
using Banking.Infrastructure.Repositories;
using Banking.Infrastructure.Services;
using Banking.Shared.Helpers;
using Banking.WPF.Stores;
using Banking.WPF.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using System.IO;
using System.Linq;
using System.Windows;
using wpf = Banking.WPF.Services;

namespace Banking.WPF
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        private readonly IConfiguration Configuration;

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
            Configuration = AddConfiguration();
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
            initialNavigationService.Navigate();

            var window = new MainWindow();
            window.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();
            window.Show();
            base.OnStartup(e);
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton(AddConfiguration());
            services.AddSingleton(x =>
            {
                var connectionString = Configuration?.GetSection("ConnectionString").Value;
                return new ConnectionString(connectionString);
            });
            services.AddSingleton(x =>
            {
                var exchangeConfigs = Configuration.GetSection("ExchangeRates").GetChildren();
                return new ExchangeRatesConfigurations(
                        exchangeConfigs.First(x => x.Key == "Url").Value,
                        exchangeConfigs.First(x => x.Key == "AccessKey").Value
                    );
            });
            services.AddSingleton<INavigationStore, NavigationStore>();
            services.AddTransient<wpf.IAuthenticationService, wpf.AuthenticationService>();
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ClientStore>();
            services.AddSingleton<AccountsStore>();
            services.AddSingleton<INavigationService>(s => CreateLoginNavigationService());

            services.AddDbContext<ClientContext>();
            services.AddTransient<ClientContext>();

            services.AddHttpClient();

            services.AddTransient<IFacade, Facade>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<wpf.IAccountService, wpf.AccountService>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IQueryRepository, ClientQueryRepository>();
            services.AddTransient<IExchangeRatesService, ExchangeRatesService>();
            services.AddTransient<IFileExportService, FileExportService>();

            services.AddSingleton<AccountTypeProviderFactory>();

            services.AddTransient<LoginViewModel>(x => {
                return new LoginViewModel(
                        x.GetRequiredService<ClientStore>(),
                        CreateAccountsNavigationService(),
                        x.GetRequiredService<wpf.IAuthenticationService>()
                    );
            });

            services.AddTransient<AccountsViewModel>(x => {
                return new AccountsViewModel(
                        x.GetRequiredService<AccountsStore>(),
                        CreateAccountsNavigationService(),
                        x.GetRequiredService<wpf.IAccountService>(),
                        x.GetRequiredService<ClientStore>()
                    );
            });

            services.AddSingleton<MainViewModel>();

            services.AddSingleton(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });
        }

        private INavigationService CreateLoginNavigationService()
        {
            return new NavigationService<LoginViewModel>(
                _serviceProvider.GetRequiredService<NavigationStore>(),
                () => _serviceProvider.GetRequiredService<LoginViewModel>());
        }
        private INavigationService CreateAccountsNavigationService()
        {
            return new NavigationService<AccountsViewModel>(
                _serviceProvider.GetRequiredService<NavigationStore>(),
                () => _serviceProvider.GetRequiredService<AccountsViewModel>());
        }
    }
}
