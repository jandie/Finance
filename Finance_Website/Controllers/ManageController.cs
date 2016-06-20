using System;
using System.Web.Mvc;
using Finance_Website.Models.Utilities;
using Library.Classes;
using Library.Interfaces;
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

        #region Balance

        [HttpGet]
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

        [HttpPost]
        public ActionResult Balance(int id, string name, decimal balanceAmount)
        {
            if (!UserUtility.UserIsValid(Session["User"] as User))
                return RedirectToAction("Login", "Account");

            try
            {
                User user = Session["User"] as User;

                user = DataRepository.Instance.Login(user.Email, Session["Password"] as string, true, false, false);

                Balance balance = user.BankAccounts.Find(b => b.Id == id);

                if (balance != null)
                {
                    ChangeRepository.Instance.ChangeBalance(id, name, balanceAmount);

                    Session["Message"] = "Balance changed.";
                }
                else
                {
                    Session["Exception"] = "Balance could not be changed.";
                }
            }
            catch (Exception)
            {
                Session["Exception"] = "Balance could not be changed.";
            }

            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        public ActionResult DeleteBalance(int id)
        {
            if (!UserUtility.UserIsValid(Session["User"] as User))
                return RedirectToAction("Login", "Account");

            try
            {
                User user = Session["User"] as User;

                user = DataRepository.Instance.Login(user.Email, Session["Password"] as string, true, false, false);

                Balance balance = user.BankAccounts.Find(b => b.Id == id);

                if (balance != null)
                {
                    DeleteRepository.Instance.DeleteBalance(id);

                    Session["Message"] = "Balance deleted.";
                }
                else
                {
                    Session["Exception"] = "Balance could not be deleted.";
                }
            }
            catch (Exception)
            {
                Session["Exception"] = "Balance could not be deleted.";
            }

            return RedirectToAction("Index", "Account");
        }

        #endregion

        #region Payment

        [HttpGet]
        public ActionResult Payment(int id = 0, string type = "", string lastTab = null)
        {
            if (!UserUtility.UserIsValid(Session["User"] as User))
                return RedirectToAction("Login", "Account");

            Session["LastTab"] = lastTab;

            try
            {
                User user = Session["User"] as User;

                user = DataRepository.Instance.Login(user.Email, Session["Password"] as string, false, true, false);

                ViewBag.User = user;
                ViewBag.Payment = user.Payments.Find(p => p.Id == id);
                ViewBag.Type = type;
                Session["User"] = user;
            }
            catch (Exception)
            {
                Session["Exception"] = "Kon gegevens niet laden";
            }

            return View();
        }

        [HttpPost]
        public ActionResult Payment(int id, string name, decimal amount)
        {
            if (!UserUtility.UserIsValid(Session["User"] as User))
                return RedirectToAction("Login", "Account");

            try
            {
                User user = Session["User"] as User;

                user = DataRepository.Instance.Login(user.Email, Session["Password"] as string, false, true, false);

                IPayment payment = user.Payments.Find(p => p.Id == id);

                if (payment != null)
                {
                    ChangeRepository.Instance.ChangePayment(id, name, amount);

                    Session["Message"] = "Payment changed.";
                }
                else
                {
                    Session["Exception"] = "Payment could not be changed.";
                }
            }
            catch (Exception)
            {
                Session["Exception"] = "Payment could not be changed.";
            }

            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        public ActionResult DeletePayment(int id)
        {
            if (!UserUtility.UserIsValid(Session["User"] as User))
                return RedirectToAction("Login", "Account");

            try
            {
                User user = Session["User"] as User;

                user = DataRepository.Instance.Login(user.Email, Session["Password"] as string, false, true, false);

                IPayment payment = user.Payments.Find(p => p.Id == id);

                if (payment != null)
                {
                    DeleteRepository.Instance.DeletePayment(id);

                    Session["Message"] = "Payment deleted.";
                }
                else
                {
                    Session["Exception"] = "Payment could not be deleted.";
                }
            }
            catch (Exception)
            {
                Session["Exception"] = "Payment could not be deleted.";
            }

            return RedirectToAction("Index", "Account");
        }

        #endregion Payment
    }
}