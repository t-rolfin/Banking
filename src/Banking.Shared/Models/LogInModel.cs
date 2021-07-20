using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Models
{
    public record LogInModel( 
        [Required] string CNP, 
        [Required] string PIN
        )
    {}
}
