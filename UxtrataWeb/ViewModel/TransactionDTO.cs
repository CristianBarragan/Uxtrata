using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UxtrataWeb.ModelView
{
    public class TransactionDTO
    {
        public int TransactionID { get; set; }
        public string TransactionType { get; set; }
        public string CourseName { get; set; }
        public string DebitAccountCode { get; set; }
        public string CreditAccountCode { get; set; }
        public decimal Amount { get; set; }
    }
}