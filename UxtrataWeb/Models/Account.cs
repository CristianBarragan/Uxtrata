using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UxtrataWeb.Models
{
    public partial class Account
    {
        public int AccountID { get; set;}
        public decimal Balance { get; set; }
        public string Code { get; set; }
    }
}