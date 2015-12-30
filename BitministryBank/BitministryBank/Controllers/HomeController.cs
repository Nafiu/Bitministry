using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BitministryBank.Models;
using BitministryBank.Repositry;
using PagedList;


namespace BitministryBank.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class HomeController : Controller
    {

        private IBankif ib;

        public HomeController()
        {
            this.ib = new BankClass(new BankAccountDb());
        }

        public ActionResult MyProfile()
        {
            BankAccount balance = ib.profile();
            List<BankAccount> b = new List<BankAccount>();
            b.Add(balance);
            return View(b);
        }

        [HttpGet]
        public ActionResult Transfer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transfer(tranferamount ta)
        {           
            if (ModelState.IsValid)
            {               
                var i=  ib.Transfer(ta);
                ib.save();
                ModelState.Remove("Amount");
                ModelState.Remove("To");                       
                if (i== "completed")
                {                   
                    ViewBag.cla = "panel-success";
                    ViewBag.headi = "Transaction";
                    ViewBag.transfer = " Transaction completed Sucessfully";
                    return View("Partialviews");
                }
                if(i== "Failed")
                {
                    ModelState.Remove("Amount");
                    ModelState.Remove("To");
                    ViewBag.cla = "panel-warning";
                    ViewBag.headi = "Transaction";
                    ViewBag.transfer = " Transaction Failed";
                    return View("Partialviews");
                }
                ViewBag.cla = "panel-warning";
                ViewBag.headi = "Transaction";
                ViewBag.transfer = " Transaction Failed due to receiver account is not available (or) you not having the balance";
                ModelState.Remove("Amount");
                ModelState.Remove("To");
                return View("Partialviews");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Deposite()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposite(tranferamount ta)
        {
            if (ModelState.IsValid)
            {
                ib.Deposite(ta);
                ib.save();
                ViewBag.cla = "panel-success";
                ViewBag.headi = "Deposite";
                ViewBag.transfer = "Deposited sucessfully";
                return View("Partialviews");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdraw(tranferamount ta)
        {           
            if (ModelState.IsValid)
            {
                var i= ib.Withdraw(ta);
                ib.save();
                if(i == "Completed")
                {
                    ViewBag.cla = "panel-success";
                    ViewBag.headi = "Withdraw";
                    ViewBag.transfer = "Withdraw sucessfully";
                    return View("Partialviews");
                }
                ViewBag.cla = "panel-warning";
                ViewBag.headi = "Withdraw";
                ViewBag.transfer = "Withdraw Failed Check your account name and your balance";
                return View("Partialviews");
            }
            return View();
        }

        public ActionResult Statement(string fromd, string tod, int? page)
        {
            BankAccountDb bae = new BankAccountDb();
            DateTime f = Convert.ToDateTime(fromd, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            DateTime t = Convert.ToDateTime(tod, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            ViewBag.fd = fromd; ViewBag.td = tod;
            var ss = bae.transdetails.Where(a => a.Username.Equals(User.Identity.Name) &&
            (a.Datetimes >= f && a.Datetimes <= t)).ToList().ToPagedList(page ?? 1, 5);
            if (ss.Count == 0 && fromd != null)
            {
                ViewBag.er = "No Transaction Occured";
                if (f > t)
                {
                    ViewBag.dat = "Choose correct Date";
                }
            }
            return View(ss);
        }

    }
}