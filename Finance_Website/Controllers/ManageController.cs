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
            ViewBag.Balance = _userUtility.User.GetBalance(id);

            return View();
        }

        public ActionResult ChangeBalance(int id, string name, decimal balanceAmount)
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            if (new ChangeRepository().ChangeBalance(_userUtility.User, id, name,
                    balanceAmount, _userUtility.Password))
            {
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

            if (new DeleteRepository().DeleteBalance(_userUtility.User, id))
            {
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

            ViewBag.Payment = _userUtility.User.GetPayment(id);

            return View();
        }

        public ActionResult ChangePayment(int id, string name, decimal amount)
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            if (new ChangeRepository().ChangePayment(_userUtility.User, id, name, amount, 
                _userUtility.Password))
            {
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

            if (new DeleteRepository().DeletePayment(_userUtility.User, id))
            {
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

            IPayment payment = _userUtility.User.GetPaymentByTransaction(id);

            ViewBag.Transaction = _userUtility.User.GetTransaction(id);
            ViewBag.PaymentId = payment?.Id;
            ViewBag.PaymentType = payment?.PaymentType.ToString();

            return View();
        }

        [HttpPost]
        public ActionResult Transaction(int id, decimal amount, string description)
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            if (new ChangeRepository().ChangeTransaction(_userUtility.User, id, amount, description,
                _userUtility.Password))
            {
                Session["Message"] = _userUtility.Language.GetText(55);
            }
            else
            {
                Session["Exception"] = _userUtility.Language.GetText(47);
            }

            return RedirectToAction("Payment", "Manage",
                new {id = _userUtility.User.GetPaymentByTransaction(id)?.Id, lastTab = Session["LastTab"]});
        }

        [HttpGet]
        public ActionResult DeleteTransaction(int id, bool quick = false)
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            if (new DeleteRepository().DeleteTransaction(_userUtility.User, id))
            {
                Session["Message"] = _userUtility.Language.GetText(56);
            }
            else
            {
                Session["Exception"] = _userUtility.Language.GetText(47);
            }

            IPayment payment = _userUtility.User.GetPaymentByTransaction(id);

            if (quick || payment == null)
                return RedirectToAction("Index", "Account");

            return RedirectToAction("Payment", "Manage",
                new {id = payment.Id, lastTab = Session["LastTab"]});
        }

        #endregion Transaction
    }
}