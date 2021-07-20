using Banking.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Models
{
    public class RegisterModel
    {
        public RegisterModel()
        { }

        public RegisterModel(string cNP, string pIN, string pINConfig, string firstName, 
            string lastName, string address, CurrencyType currencyType, AccountTypeEnum accountType)
        {
            CNP = cNP;
            PIN = pIN;
            PINConfig = pINConfig;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            CurrencyType = currencyType;
            AccountType = accountType;
        }

        [MaxLength(13, ErrorMessage = "The CNP must have a length of 13.")]
        [MinLength(13, ErrorMessage = "The CNP must have a length of 13.")]
        public string CNP { get; set; }

        [DataType(DataType.Password)]
        public string PIN { get; set; }

        [Compare("PIN")]
        [DataType(DataType.Password)]
        [Display(Name = "Pin Confirm")]
        public string PINConfig { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public CurrencyType CurrencyType { get; set; } = CurrencyType.RON;
        public AccountTypeEnum AccountType { get; set; } = AccountTypeEnum.Basic;
    }
}
