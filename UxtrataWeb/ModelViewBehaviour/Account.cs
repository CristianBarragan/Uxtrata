using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UxtrataWeb.Models
{
    [MetadataType(typeof(Account.MetaData))]
    public partial class Account
    {
        internal class MetaData
        {
            [Required(ErrorMessage = "Required")]
            [Range(0.0, Double.MaxValue, ErrorMessage = "Please enter a value greater than 0")]
            [DisplayName("Account Balance")]
            public decimal Balance { get; set; }

            [RegularExpression(@"^.{5,}$", ErrorMessage = "Minimum 3 characters required")]
            [Required(ErrorMessage = "Required")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
            [DisplayName("Code")]
            public string Code { get; set; }
        }
    }
}