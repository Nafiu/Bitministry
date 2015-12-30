using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BitministryBank.Models
{
    public class Transactionsdetail
    {
        public int id { get; set; }
        public string Username { get; set; }
        [Display(Name = "Name")]
        public string Tousername { get; set; }
        [Display(Name = "Date & Time")]
        public Nullable<System.DateTime> Datetimes { get; set; }
        public Nullable<decimal> Debited { get; set; }
        public Nullable<decimal> Credited { get; set; }
        [Display(Name = "Type")]
        public string Typeofaction { get; set; }
        public Nullable<double> Balance { get; set; }

        public virtual BankAccount BankAccount { get; set; }
    }
}