using Banking.Core.Entities;
using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.AccountTypeFactory
{
    public class AccountTypeProviderFactory
    {
        private readonly List<IAccountType> accountType;

        public AccountTypeProviderFactory()
        {
            var accountTypeProviderType = typeof(IAccountType);
            accountType = accountTypeProviderType.Assembly
                .ExportedTypes.Where(x => x.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => {
                    var parameterlessCtor = x.GetConstructors().SingleOrDefault(y => y.GetParameters().Length == 0);
                    _ = parameterlessCtor ?? throw new ArgumentNullException();
                    return Activator.CreateInstance(x);
                }).Cast<IAccountType>().ToList();
        }

        public AccountType GetAccountTypeByType(AccountTypeEnum accountType)
        {
            return this.accountType.Find(x => x.AccountType == accountType).GetAccountType();
        }
    }
}
