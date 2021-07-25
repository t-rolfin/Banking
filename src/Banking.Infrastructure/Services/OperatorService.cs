using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Services
{
    public class OperatorService : IOperatorService
    {
        private Operator _operator;

        public OperatorService()
        {
            _operator = new Operator("Operator 1", "1996", "Th3Operator", "Operator");
        }

        public Operator IdentifyOperator(string employeeId, string password)
        {
            if (string.IsNullOrWhiteSpace(employeeId) || string.IsNullOrWhiteSpace(password))
                return null;

            if (_operator.Password == password && _operator.EmployeeId == employeeId)
            {

                return new Operator(_operator.FullName, "", "", _operator.Role);
            }
            else
                return null;
        }

    }


    public class Operator
    {
        public Operator(string fullName, string employeeId, string password, string role)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            EmployeeId = employeeId;
            Password = password;
            Role = role;
        }

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string EmployeeId { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
