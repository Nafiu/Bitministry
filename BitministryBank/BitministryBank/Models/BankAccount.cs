using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitministryBank.Models
{
    public class BankAccount
    {
        public int id { get; set; }
        public string Username { get; set; }
        public string password { get; set; }
        public Nullable<double> AccountBalance { get; set; }
        public virtual ICollection<Transactionsdetail> Transactionsdetails { get; set; }
    }
}