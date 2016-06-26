using System;
using System.Linq;
using System.Web.Mvc;
using Finance_Website.Models.Utilities;
using Library.Classes;
using Library.Interfaces;
using Repository;

namespace Finance_Website.Controllers
{
    public class ManageController : Controller
    {
        private IPayment _payment;

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
        
        public ActionResult ChangeBalance(int id, string name, decimal balanceAmount)
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

            if (string.IsNullOrWhiteSpace(Session["LastTab"] as string))
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
        public ActionResult Payment(int id = 0, string lastTab = null)
        {
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            Session["LastTab"] = lastTab;

            try
            {
                _payment = user.Payments.Find(p => p.Id == id);

                ViewBag.Payment = _payment;
            }
            catch (Exception)
            {
                Session["Exception"] = "Kon gegevens niet laden";
            }

            return View();
        }
        
        public ActionResult ChangePayment(int id, string name, decimal amount)
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

            if (string.IsNullOrWhiteSpace(Session["LastTab"] as string))
                Session["LastTab"] = lastTab;

            try
            {
                _payment = user.Payments.Find(p => p.Id == id);

                if (_payment != null)
                {
                    DeleteRepository.Instance.DeletePayment(_payment.Id);

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
        public ActionResult Transaction(int id = 0)
        {
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            try
            {
                _payment = user.Payments.Find(p => p.AllTransactions.Any(t => t.Id == id));

                Transaction transaction =_payment.AllTransactions.Find(t => t.Id == id);

                ViewBag.Transaction = transaction;
                ViewBag.PaymentId = _payment.Id;
                ViewBag.PaymentType = _payment.PaymentType.ToString();
            }
            catch (Exception)
            {
                Session["Exception"] = "Data could not be loaded";
            }

            return View();
        }

        [HttpPost]
        public ActionResult Transaction(int id, decimal amount, string description)
        {
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            try
            {
                _payment = user.Payments.Find(p => p.AllTransactions.Any(t => t.Id == id));

                Transaction transaction =_payment.AllTransactions.Find(t => t.Id == id);

                if (transaction != null)
                {
                    ChangeRepository.Instance.ChangeTransaction(id, amount, description);

                    Session["Message"] = "Transaction changed.";
                }
                else
                    Session["Exception"] = "Transaction could not be changed.";
            }
            catch (Exception)
            {
                Session["Exception"] = "Transaction could not be changed.";
            }

            return RedirectToAction("Payment", "Manage",
                new {id = _payment.Id, lastTab = Session["LastTab"]});
        }

        [HttpGet]
        public ActionResult DeleteTransaction(int id, bool quick = false)
        {
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            try
            {
                _payment = user.Payments.Find(p => p.AllTransactions.Any(t => t.Id == id));

                Transaction transaction = _payment.AllTransactions.Find(t => t.Id == id);

                if (transaction != null)
                {
                    DeleteRepository.Instance.DeleteTransaction(id);

                    Session["Message"] = "Transaction deleted.";
                }
                else
                    Session["Exception"] = "Transaction could not be deleted.";
            }
            catch (Exception)
            {
                Session["Exception"] = "Transaction could not be deleted.";
            }

            if (quick || _payment == null)
                return RedirectToAction("Index", "Account");
            
            return RedirectToAction("Payment", "Manage",
                new { id = _payment.Id, lastTab = Session["LastTab"] });
        }

        #endregion Transaction
    }
}