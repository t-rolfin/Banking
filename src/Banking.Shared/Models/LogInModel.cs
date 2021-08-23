using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Models
{
    public class LogInModel {

        public LogInModel()
        {

        }

        public LogInModel(string cNP, string pIN)
        {
            CNP = cNP;
            PIN = pIN;
        }

        [Required]
        public string CNP { get; set; }

        [Required] 
        public string PIN { get; set; }
    }
}
