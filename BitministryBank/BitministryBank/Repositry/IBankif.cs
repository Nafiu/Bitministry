using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitministryBank.Models;

namespace BitministryBank.Repositry
{
    interface IBankif
    {
        BankAccount profile();
        string Transfer(tranferamount ta);
        string Withdraw(tranferamount ta);
        void Deposite(tranferamount ta);
        void save();
    }
}
