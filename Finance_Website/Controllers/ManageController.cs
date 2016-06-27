using System;
using System.Linq;
using System.Web.Mvc;
using Library.Classes;
using Library.Exceptions;
using Library.Interfaces;
using Repository;

namespace Finance_Website.Controllers
{
    public class ManageController : Controller
    {
        private IPayment _payment;

        private User _user;

        public void InitializeAction(string lastTab = null)
        {
            if (string.IsNullOrWhiteSpace(Session["LastTab"] as string))
                Session["LastTab"] = lastTab;

            _user = null;

            _user = Session["User"] as User;

            if (_user == null) throw new WrongUsernameOrPasswordException("Not logged in.");

            _user = DataRepository.Instance.Login(_user.Email, Session["Password"] as string, true, true, true);
        }

        // GET: Manage
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Account");
        }

        #region Balance

        [HttpGet]
        public ActionResult Balance(int id = 0, string lastTab = null)
        {
            try
            {
                InitializeAction(lastTab);
            }
            catch (WrongUsernameOrPasswordException x)
            {
                Session["Exception"] = x.Message;

                return RedirectToAction("Login", "Account");
            }

            try
            {
                ViewBag.User = _user;
                ViewBag.Balance = _user.Balances.Find(b => b.Id == id);
            }
            catch (Exception)
            {
                Session["Exception"] = "Kon gegevens niet laden";
            }

            return View();
        }
        
        public ActionResult ChangeBalance(int id, string name, decimal balanceAmount)
        {
            try
            {
                InitializeAction();
            }
            catch (WrongUsernameOrPasswordException x)
            {
                Session["Exception"] = x.Message;

                return RedirectToAction("Login", "Account");
            }

            try
            {
                Balance balance = _user.Balances.Find(b => b.Id == id);

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
            try
            {
                InitializeAction(lastTab);
            }
            catch (WrongUsernameOrPasswordException x)
            {
                Session["Exception"] = x.Message;

                return RedirectToAction("Login", "Account");
            }

            try
            {
                Balance balance = _user.Balances.Find(b => b.Id == id);

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
            try
            {
                InitializeAction(lastTab);
            }
            catch (WrongUsernameOrPasswordException x)
            {
                Session["Exception"] = x.Message;

                return RedirectToAction("Login", "Account");
            }

            try
            {
                _payment = _user.Payments.Find(p => p.Id == id);

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
            try
            {
                InitializeAction();
            }
            catch (WrongUsernameOrPasswordException x)
            {
                Session["Exception"] = x.Message;

                return RedirectToAction("Login", "Account");
            }

            try
            {
                IPayment payment = _user.Payments.Find(p => p.Id == id);

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
            try
            {
                InitializeAction(lastTab);
            }
            catch (WrongUsernameOrPasswordException x)
            {
                Session["Exception"] = x.Message;

                return RedirectToAction("Login", "Account");
            }

            try
            {
                _payment = _user.Payments.Find(p => p.Id == id);

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
            try
            {
                InitializeAction();
            }
            catch (WrongUsernameOrPasswordException x)
            {
                Session["Exception"] = x.Message;

                return RedirectToAction("Login", "Account");
            }

            try
            {
                _payment = _user.Payments.Find(p => p.AllTransactions.Any(t => t.Id == id));

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
            try
            {
                InitializeAction();
            }
            catch (WrongUsernameOrPasswordException x)
            {
                Session["Exception"] = x.Message;

                return RedirectToAction("Login", "Account");
            }

            try
            {
                _payment = _user.Payments.Find(p => p.AllTransactions.Any(t => t.Id == id));

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
            try
            {
                InitializeAction();
            }
            catch (WrongUsernameOrPasswordException x)
            {
                Session["Exception"] = x.Message;

                return RedirectToAction("Login", "Account");
            }

            try
            {
                _payment = _user.Payments.Find(p => p.AllTransactions.Any(t => t.Id == id));

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