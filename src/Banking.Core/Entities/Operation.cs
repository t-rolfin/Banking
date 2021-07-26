using Banking.Shared.Enums;

namespace Banking.Core.Entities
{
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
}
