using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UxtrataWeb.Models
{
    public partial class Transaction
    {
        public int TransactionID { get; set; }
        public int DebitAccountID { get; set; }
        public int CreditAccountID { get; set; }
        public int BusinessID { get; set; }
        public int BusinessTypeID { get; set; }
        public decimal Amount { get; set; }

        public virtual Account DebitAccount { get; set; }
        public virtual Account CreditAccount { get; set; }
        public virtual BusinessType BusinessType { get; set; }
    }
}