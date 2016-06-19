using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finance_Website.Models.Utilities;
using Library.Classes;
using Repository;

namespace Finance_Website.Controllers
{
    public class ManageController : Controller
    {
        // GET: Manage
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Balance(int id = 0)
        {
            if (!UserUtility.UserIsValid(Session["User"] as User))
                return RedirectToAction("Login", "Account");

            try
            {
                User user = Session["User"] as User;

                user = DataRepository.Instance.Login(user.Email, Session["Password"] as string, true, false, false);

                ViewBag.User = user;
                ViewBag.Balance = user.BankAccounts.Find(b => b.Id == id);
                Session["User"] = user;
            }
            catch (Exception)
            {
                Session["Exception"] = "Kon gegevens niet laden";
            }

            return View();
        }

        public ActionResult Balance(int id, string name, decimal balanceAmount)
        {
            if (!UserUtility.UserIsValid(Session["User"] as User))
                return RedirectToAction("Login", "Account");

            try
            {
                User user = Session["User"] as User;

                user = DataRepository.Instance.Login(user.Email, Session["Password"] as string, true, false, false);

                ViewBag.User = user;
                ViewBag.Balance = user.BankAccounts.Find(b => b.Id == id);

                Session["User"] = user;
            }
            catch (Exception)
            {
                Session["Exception"] = "Kon gegevens niet laden";
            }

            return View();
        }
    }
}