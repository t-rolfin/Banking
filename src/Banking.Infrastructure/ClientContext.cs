using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Banking.Shared.Helpers;

namespace Banking.Infrastructure
{
    public class ClientContext : DbContext
    {
        private readonly ConnectionString _connectionString;

        public ClientContext(ConnectionString connectionString)
            :base()
        {
            _connectionString = connectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString.Value, x => x.MigrationsAssembly("Banking.WebUI"))
                .UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

    }
}
