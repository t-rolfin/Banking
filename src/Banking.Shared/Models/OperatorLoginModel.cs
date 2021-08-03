using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Models
{
    public class OperatorLoginModel
    {
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
