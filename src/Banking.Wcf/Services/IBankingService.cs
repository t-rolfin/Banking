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
    [ServiceContract]
    public interface IBankingService
    {
        [OperationContract(AsyncPattern = true)]
        [FaultContract(typeof(FaultException<ExceptionDetail>))]
        Task<string> CreateAccount(string clientCNP, string PIN, string firstName, string lastName,
            AccountTypeEnum accountType, CurrencyType currencyType);
    }
}
