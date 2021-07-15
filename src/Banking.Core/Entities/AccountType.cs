using Banking.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.Entities
{
    public class AccountType
    {
        public AccountType(string name)
        {
            Name = name;
        }

        public string Name { get; init; }
        public bool HasCommisions { get; protected set; }
        public List<Operation> Operations { get; } = new();


        public void AddOperation(OperationType operationType, Commission commission)
        {
            this.Operations.Add(
                    new Operation(operationType, commission)
                );

            this.HasCommisions = true;
        }
    }
}
