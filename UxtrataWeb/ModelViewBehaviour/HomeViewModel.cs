using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UxtrataWeb.ModelView
{
    [MetadataType(typeof(HomeViewModel.MetaData))]
    public partial class HomeViewModel
    {
        internal class MetaData
        {
            [Required(ErrorMessage = "Required")]
            [Range(0.0, Double.MaxValue, ErrorMessage = "Please enter a value greater than 0")]
            [DisplayName("Value")]
            public decimal PaymentValue { get; set; }
        }
    }
}