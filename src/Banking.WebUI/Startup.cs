using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.Core;
using Banking.Core.AccountTypeFactory;
using Banking.Core.Interfaces;
using Banking.Core.Repositories;
using Banking.Infrastructure;
using Banking.Infrastructure.Repositories;
using Banking.Infrastructure.Services;
using Banking.Shared.Helpers;
using Banking.Shared.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Banking.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSingleton(x => {
                var connectionString = Configuration.GetConnectionString("clientConnectionString");
                return new ConnectionString(connectionString);
            });

            services.AddDbContext<ClientContext>();
            services.AddTransient<ClientContext>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(x =>
                {
                    x.LoginPath = "/Account/LogIn";
                });

            services.AddAuthorization(o =>
            {
                o.AddPolicy("Operator", policy => policy.Requirements.Add(new HasRoleRequirement("Operator")));
            });

            services.AddTransient<IFacade, Facade>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IQueryRepository, ClientQueryRepository>();

            services.AddSingleton<IAuthorizationHandler, HasRoleHandler>();
            services.AddSingleton<AccountTypeProviderFactory>();
            services.AddSingleton<IOperatorService, OperatorService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
