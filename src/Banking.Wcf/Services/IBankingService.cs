using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Banking.Wcf.Services
{
    [ServiceContract]
    public interface IBankingService
    {
        [OperationContract]
        string DoWork();
    }
}
