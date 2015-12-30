using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitministryBank.Models;
using System.Transactions;


namespace BitministryBank.Repositry
{
    public class BankClass : IBankif
    {
        private BankAccountDb db;
        public BankClass(BankAccountDb _db)
        {
            this.db = _db;
        }

        string username = HttpContext.Current.User.Identity.Name;  

        //username profile
        public BankAccount profile()
        {           
            BankAccount balance = db.Generaltable.Where(a => a.Username.Equals(username)).FirstOrDefault();
            return balance;
        }

        //Transfer one account to another account
        public string Transfer(tranferamount ta)
        {
            bool debitmoney = false;
            bool creditmoney = false;
            try
            {
                using (TransactionScope ts = new TransactionScope())      //Transaction scope
                {
                    var from = profile();
                    var to = db.Generaltable.Where(a => a.Username.Equals(ta.To)).FirstOrDefault();
                    if (from.AccountBalance >= ta.Amount && to != null)
                    {
                        BankAccount bafrom = from;               //in Bankaccount table Debited our amount
                        bafrom.AccountBalance = bafrom.AccountBalance - ta.Amount;

                        BankAccount bato = to;                   //in Bankaccount Credited to amount
                        bato.AccountBalance = bato.AccountBalance + ta.Amount;                  

                        Transactionsdetail tddebit = new Transactionsdetail();     //Transaction history for debited
                        tddebit.Username = username;
                        tddebit.Tousername = ta.To;
                        tddebit.Datetimes = DateTime.Now;
                        tddebit.Debited = ta.Amount;
                        tddebit.Credited = null;
                        tddebit.Balance = bafrom.AccountBalance;
                        tddebit.Typeofaction = "Transaction";
                        db.transdetails.Add(tddebit);

                        Transactionsdetail tdcredit = new Transactionsdetail();     //Transaction history for credited
                        tdcredit.Username = ta.To;
                        tdcredit.Tousername = username;
                        tdcredit.Datetimes = DateTime.Now;
                        tdcredit.Debited = null;
                        tdcredit.Credited = ta.Amount;
                        tdcredit.Typeofaction = "Transaction";
                        tdcredit.Balance = bato.AccountBalance;
                        db.transdetails.Add(tdcredit);
                        debitmoney = true;
                        creditmoney = true;
                        if (debitmoney && creditmoney)        //all done correctly it will go on this loop                          
                        {
                            ts.Complete();
                            return "completed";
                        }
                    }
                }
            }
            catch
            {
                return "Failed";         //any error came in try it will fail the transaction              
            }
            return "no";
        }
        public void Deposite(tranferamount ta)
        {
            var userdeposite = profile();
            BankAccount ba = userdeposite;
            ba.AccountBalance = ba.AccountBalance + ta.Amount;   //Credited user amount

            Transactionsdetail td = new Transactionsdetail();    //Transaction history
            td.Username = ta.Username;
            td.Typeofaction = "Deposit";
            td.Tousername = ta.To;
            td.Credited = ta.Amount;
            td.Datetimes = DateTime.Now;
            td.Balance = ba.AccountBalance;
            db.transdetails.Add(td);
        }
        public string Withdraw(tranferamount ta)
        {
            var userwithdraw = profile();
            if (userwithdraw.AccountBalance >= ta.Amount)             //check User having that much amount
            {
                BankAccount ba = userwithdraw;                       //Debited user amount
                ba.AccountBalance = ba.AccountBalance - ta.Amount;

                Transactionsdetail td = new Transactionsdetail();    //Transaction history
                td.Username = ta.Username;
                td.Tousername = ta.To;
                td.Typeofaction = "Withdraw";
                td.Balance = ba.AccountBalance;
                td.Debited = ta.Amount;
                td.Datetimes = DateTime.Now;
                db.transdetails.Add(td);
                return "Completed";
            }
            return "Failed";
        }
        public void save()
        {
            db.SaveChanges();   //Saving db
        }
      
    }
}