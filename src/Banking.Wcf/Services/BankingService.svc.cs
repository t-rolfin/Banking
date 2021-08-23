using Banking.Core;
using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Wcf.Services
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall
    )]
    public class BankingService : IBankingService
    {

        private readonly IFacade _facade;

        public BankingService(IFacade facade)
        {
            _facade = facade;
        }

        [OperationBehavior]
        public async Task<string> CreateAccount(string clientCNP, string PIN, string firstName, string lastName,
            AccountTypeEnum accountType, CurrencyType currencyType)
        {
            var response = await _facade.RegisterClient(clientCNP, PIN, firstName, lastName, string.Empty, accountType, currencyType);
            return response.MetaResult.Message;
        }
    }
}
