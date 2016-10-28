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
        private SessionUtility _userUtility;

        public void InitializeAction(string lastTab = null)
        {
            _userUtility = SessionUtility.InitializeUtil(Session["UserUtility"], lastTab);

            Session["UserUtility"] = _userUtility;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Account");
        }

        #region Balance

        [HttpGet]
        public ActionResult Balance(int id = 0, string lastTab = null)
        {
            InitializeAction(lastTab);

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            ViewBag.User = _userUtility.User;
            ViewBag.Balance = _userUtility.User.Balances.Find(b => b.Id == id);

            return View();
        }

        public ActionResult ChangeBalance(int id, string name, decimal balanceAmount)
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            Balance balance = _userUtility.User.Balances.Find(b => b.Id == id);

            if (balance != null)
            {
                ChangeRepository.Instance.ChangeBalance(balance, name, balanceAmount);

                Session["Message"] = _userUtility.Language.GetText(51);
            }
            else
            {
                Session["Exception"] = _userUtility.Language.GetText(47);
            }

            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        public ActionResult DeleteBalance(int id, string lastTab = null)
        {
            InitializeAction(lastTab);

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            Balance balance = _userUtility.User.Balances.Find(b => b.Id == id);

            if (balance != null)
            {
                DeleteRepository.Instance.DeleteBalance(_userUtility.User, id);

                Session["Message"] = _userUtility.Language.GetText(52);
            }
            else
            {
                Session["Exception"] = _userUtility.Language.GetText(47);
            }

            return RedirectToAction("Index", "Account");
        }

        #endregion

        #region Payment

        [HttpGet]
        public ActionResult Payment(int id = 0, string lastTab = null)
        {
            InitializeAction(lastTab);

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            _payment = _userUtility.User.Payments.Find(p => p.Id == id);

            ViewBag.Payment = _payment;

            return View();
        }

        public ActionResult ChangePayment(int id, string name, decimal amount)
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            IPayment payment = _userUtility.User.Payments.Find(p => p.Id == id);

            if (payment != null)
            {
                ChangeRepository.Instance.ChangePayment(payment, name, amount);

                Session["Message"] = _userUtility.Language.GetText(53);
            }
            else
            {
                Session["Exception"] = _userUtility.Language.GetText(47);
            }

            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        public ActionResult DeletePayment(int id, string lastTab = null)
        {
            InitializeAction(lastTab);

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            _payment = _userUtility.User.Payments.Find(p => p.Id == id);

            if (_payment != null)
            {
                DeleteRepository.Instance.DeletePayment(_userUtility.User, _payment.Id);

                Session["Message"] = _userUtility.Language.GetText(54);
            }
            else
            {
                Session["Exception"] = _userUtility.Language.GetText(47);
            }

            return RedirectToAction("Index", "Account");
        }

        #endregion Payment

        #region Transaction

        [HttpGet]
        public ActionResult Transaction(int id = 0)
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            _payment = _userUtility.User.Payments.Find(p => p.AllTransactions.Any(t => t.Id == id));

            Transaction transaction = _payment?.AllTransactions.Find(t => t.Id == id);

            ViewBag.Transaction = transaction;
            ViewBag.PaymentId = _payment?.Id;
            ViewBag.PaymentType = _payment?.PaymentType.ToString();

            return View();
        }

        [HttpPost]
        public ActionResult Transaction(int id, decimal amount, string description)
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            _payment = _userUtility.User.Payments.Find(p => p.AllTransactions.Any(t => t.Id == id));

            Transaction transaction = _payment.AllTransactions.Find(t => t.Id == id);

            if (transaction != null)
            {
                ChangeRepository.Instance.ChangeTransaction(transaction, amount, description);

                Session["Message"] = _userUtility.Language.GetText(55);
            }
            else
                Session["Exception"] = _userUtility.Language.GetText(47);

            return RedirectToAction("Payment", "Manage",
                new {id = _payment.Id, lastTab = Session["LastTab"]});
        }

        [HttpGet]
        public ActionResult DeleteTransaction(int id, bool quick = false)
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            _payment = _userUtility.User.Payments.Find(p => p.AllTransactions.Any(t => t.Id == id));

            Transaction transaction = _payment.AllTransactions.Find(t => t.Id == id);

            if (transaction != null)
            {
                DeleteRepository.Instance.DeleteTransaction(_payment as Payment, id);

                Session["Message"] = _userUtility.Language.GetText(56);
            }
            else
                Session["Exception"] = _userUtility.Language.GetText(47);

            if (quick || _payment == null)
                return RedirectToAction("Index", "Account");

            return RedirectToAction("Payment", "Manage",
                new {id = _payment.Id, lastTab = Session["LastTab"]});
        }

        #endregion Transaction
    }
}