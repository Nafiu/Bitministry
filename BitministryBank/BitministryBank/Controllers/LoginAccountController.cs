using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BitministryBank.Models;
using System.Web.Security;
namespace BitministryBank.Controllers
{
    [AllowAnonymous]
    public class LoginAccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login l, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                string encryptpass = FormsAuthentication.HashPasswordForStoringInConfigFile(l.Password, "SHA1");
                using (BankAccountDb b = new BankAccountDb())
                {
                    var u = b.Generaltable.Where(a => a.Username.Equals(l.Username) && a.password.Equals(encryptpass)).FirstOrDefault();
                    if (u != null)
                    {
                        FormsAuthentication.SetAuthCookie(u.Username, false);
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("MyProfile", "Home");
                        }
                    }
                }
            }
            ViewBag.forpas = "Wrong Password";
            return View();
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Views");
        }
        public ActionResult Views()
        {
            return View();
        }

    }
}