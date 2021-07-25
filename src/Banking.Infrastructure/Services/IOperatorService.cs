namespace Banking.Infrastructure.Services
{
    public interface IOperatorService
    {
        Operator IdentifyOperator(string employeeId, string password);
    }
}