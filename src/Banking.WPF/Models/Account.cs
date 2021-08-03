using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.WPF.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string CardType { get; set; }
        public decimal Sold { get; set; }
    }
}
