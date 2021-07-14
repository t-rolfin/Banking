using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.ClientAggregate
{
    public class AccountType
    {
        public AccountType(string name)
        {
            Name = name;
        }

        public string Name { get; init; }
        public List<Operation> Operations { get; }

        public void AddOperation(OperationType operationType, Commission commission)
        {
            this.Operations.Add(
                    new Operation(operationType, commission)
                );
        }
    }

    public class Operation
    {
        public Operation(OperationType operationType, Commission commission)
        {
            OperationType = operationType;
            Commission = commission;
        }

        public OperationType OperationType { get; set; }
        public Commission Commission { get; set; }
    }

    public class Commission
    {
        public Commission(float percent, float @fixed)
        {
            Percent = percent;
            Fixed = @fixed;
        }

        public float Percent { get; set; }
        public float Fixed { get; set; }
    }

    public enum OperationType
    {
        Withdrawal = 0,
        Deposit = 1,
        Transfer = 2,
    }
}
