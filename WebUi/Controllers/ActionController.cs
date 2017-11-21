using System.Web.Mvc;
using FinanceLibrary.Enums;
using FinanceLibrary.Logic;
using Finance_Website.Models;
using Finance_Website.Models.Utilities;
using Newtonsoft.Json;

namespace Finance_Website.Controllers
{
    public class ActionController : Controller
    {
        private SessionUtility _userUtility;

        private void InitializeAction(string lastTab = null)
        {
            _userUtility = SessionUtility.InitializeUtil(Session["UserUtility"], lastTab);

            Session["UserUtility"] = _userUtility;
        }

        private void SaveAction()
        {
            Session["UserUtility"] = _userUtility;
        }

        public ActionResult AddBalance(string name, decimal balance, string lastTab = null)
        {
            InitializeAction(lastTab);

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            new InsertLogic().AddBankAccount(_userUtility.User, name.Trim(), balance);

            SaveAction();

            Session["Message"] = _userUtility.Language.GetText(44);

            return RedirectToAction("Index", "Account");
        }

        public ActionResult AddMonthlyBill(string name, decimal amount, string lastTab = null)
        {
            InitializeAction(lastTab);

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            new InsertLogic().AddPayment(_userUtility.User, name.Trim(), amount, PaymentType.MonthlyBill);

            SaveAction();

            Session["Message"] = _userUtility.Language.GetText(46);

            return RedirectToAction("Index", "Account");
        }

        public ActionResult AddMonthlyIncome(string name, decimal amount, string lastTab = null)
        {
            InitializeAction(lastTab);

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            new InsertLogic().AddPayment(_userUtility.User, name.Trim(), amount, PaymentType.MonthlyIncome);

            SaveAction();

            Session["Message"] = _userUtility.Language.GetText(48);

            return RedirectToAction("Index", "Account");
        }

        public ActionResult Transaction(int paymentId, string lastTab = null)
        {
            InitializeAction(lastTab);

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            ViewBag.PaymentId = paymentId;
            ViewBag.PaymentName = _userUtility.User.GetPayment(paymentId).Name;
            ViewBag.User = _userUtility.User;

            return View();
        }

        public string AddTransaction(int paymentId, string description, decimal amount, 
            int balanceId)
        {
            InitializeAction();

            if (_userUtility.User == null)
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(1),
                    Success = false,
                    LogOut = true,
                    Object = null
                });

            if (!new InsertLogic().AddTransaction(_userUtility.User, paymentId, 
                balanceId, amount, description.Trim()))
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(47),
                    Success = false,
                    Object = null
                });

            SaveAction();
            return JsonConvert.SerializeObject(new Response
            {
                Message = _userUtility.Language.GetText(50),
                Success = true,
                Object = _userUtility.User
            });
        }

        public string TestAjax(string message)
        {
            Response response = new Response
            {
                Message = "Request received!",
                Success = true,
                Object = message
            };

            return JsonConvert.SerializeObject(response);
        }
    }
}