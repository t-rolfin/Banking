using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Banking.Wcf.Services
{
    public class BankingService : IBankingService
    {
        public string DoWork()
        {
            return "This is working!";
        }
    }
}
