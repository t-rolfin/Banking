using Ardalis.GuardClauses;
using Banking.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.Entities
{
    public class AccountType
    {
        public AccountType(string name, int enumPosition)
        {
            Name = name;
            EnumPosition = enumPosition;
        }

        public string Name { get; init; }
        public int EnumPosition { get; set; }
        public bool HasCommisions { get; protected set; }
        public List<Operation> Operations { get; } = new();


        public void AddOperation(OperationType operationType, Commission commission)
        {
            Guard.Against.Null(operationType, nameof(AccountType));
            Guard.Against.Null(commission, nameof(AccountType));

            this.Operations.Add(
                    new Operation(operationType, commission)
                );

            this.HasCommisions = true;
        }
    }
}
