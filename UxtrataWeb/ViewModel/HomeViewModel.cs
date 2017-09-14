using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UxtrataWeb.ModelView
{
    public partial class HomeViewModel
    {
        public IEnumerable<SelectListItem> Students { get; set; }
        public string Student { get; set; }
        public decimal PaymentValue { get; set; }
    }
}