using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Models
{
    public class ClientModel
    {
        public ClientModel() { }

        public ClientModel(Guid id, string cnp, string fullName, string address, decimal total)
        {
            Id = id;
            CNP = cnp;
            FullName = fullName;
            Address = address;
            Total = total;
        }

        public Guid Id { get; set; }
        public string CNP { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public decimal Total { get; set; }
    }
}
