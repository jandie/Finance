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
        public ActionResult Balance(int id = 0, string lastTab = null)
        {
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            Session["LastTab"] = lastTab;

            try
            {
                ViewBag.User = user;
                ViewBag.Balance = user.Balances.Find(b => b.Id == id);
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
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            try
            {
                Balance balance = user.Balances.Find(b => b.Id == id);

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
        public ActionResult DeleteBalance(int id, string lastTab = null)
        {
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            Session["LastTab"] = lastTab;

            try
            {
                Balance balance = user.Balances.Find(b => b.Id == id);

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
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            Session["LastTab"] = lastTab;

            try
            {
                IPayment payment = user.Payments.Find(p => p.Id == id);

                ViewBag.Payment = payment;
                ViewBag.Transactions = payment.AllTransactions;
                ViewBag.Type = type;
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
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            try
            {
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
        public ActionResult DeletePayment(int id, string lastTab = null)
        {
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            Session["LastTab"] = lastTab;

            try
            {
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

        #region Transaction

        [HttpGet]
        public ActionResult Transaction(int id = 0, int paymentId = 0, string paymentType = "")
        {
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            try
            {
                user = DataRepository.Instance.Login(user.Email, Session["Password"] as string, false, true, true);

                Transaction transaction =
                    user.Payments.Find(p => p.Id == paymentId).AllTransactions.Find(t => t.Id == id);

                ViewBag.Transaction = transaction;
                ViewBag.PaymentId = paymentId;
                ViewBag.PaymentType = paymentType;
            }
            catch (Exception)
            {
                Session["Exception"] = "Data could not be loaded";
            }

            return View();
        }

        [HttpPost]
        public ActionResult Transaction(int id = 0, int paymentId = 0, string paymentType = "", decimal amount = 0,
            string description = "")
        {
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            try
            {
                Transaction transaction =
                    user.Payments.Find(p => p.Id == paymentId).AllTransactions.Find(t => t.Id == id);

                if (transaction != null)
                {
                    ChangeRepository.Instance.ChangeTransaction(id, amount, description);

                    Session["Message"] = "Transaction changed.";
                }
                else
                {
                    Session["Exception"] = "Transaction could not be changed.";
                }
            }
            catch (Exception)
            {
                Session["Exception"] = "Transaction could not be changed.";
            }

            return RedirectToAction("Payment", "Manage",
                new {id = paymentId, type = paymentType, lastTab = Session["LastTab"]});
        }

        [HttpGet]
        public ActionResult DeleteTransaction(int id, int paymentId, string paymentType)
        {
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            try
            {
                Transaction transaction =
                    user.Payments.Find(p => p.Id == paymentId).AllTransactions.Find(t => t.Id == id);

                if (transaction != null)
                {
                    DeleteRepository.Instance.DeleteTransaction(id);

                    Session["Message"] = "Transaction deleted.";
                }
                else
                {
                    Session["Exception"] = "Transaction could not be deleted.";
                }
            }
            catch (Exception)
            {
                Session["Exception"] = "Transaction could not be deleted.";
            }

            return RedirectToAction("Payment", "Manage",
                new {id = paymentId, type = paymentType, lastTab = Session["LastTab"]});
        }

        #endregion Transaction
    }
}