using System;
using System.Linq;
using System.Web.Mvc;
using Finance_Website.Models.Utilities;
using Library.Classes;
using Library.Enums;
using Library.Interfaces;
using Repository;

namespace Finance_Website.Controllers
{
    public class ActionController : Controller
    {
        private UserUtility _userUtility;

        public bool InitializeAction(string lastTab = null)
        {
            bool succes = true;

            object sessionUser = Session["User"];
            object sessionPassword = Session["Password"];
            object sessionLanguage = Session["Language"];
            object sessionLastTab = Session["LastTab"];

            try
            {
                _userUtility = new UserUtility(ref sessionUser, ref sessionPassword, ref sessionLanguage, ref sessionLastTab, lastTab);
            }
            catch (Exception)
            {
                succes = false;
            }

            Session["User"] = sessionUser;
            Session["Password"] = sessionPassword;
            Session["Language"] = sessionLanguage;
            Session["LastTab"] = sessionLastTab;

            return succes;
        }

        public ActionResult AddBalance(string name, decimal balance, string lastTab = null)
        {
            InitializeAction(lastTab);

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            InsertRepository.Instance.AddBankAccount(_userUtility.User.Id, name.Trim(), balance);

            Session["Message"] = _userUtility.Language.GetText(44);

            return RedirectToAction("Index", "Account");
        }

        public ActionResult AddMonthlyBill(string name, decimal amount, string lastTab = null)
        {
            InitializeAction(lastTab);

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            InsertRepository.Instance.AddPayment(_userUtility.User.Id, name.Trim(), amount, PaymentType.MonthlyBill);

            Session["Message"] = _userUtility.Language.GetText(46);

            return RedirectToAction("Index", "Account");
        }

        public ActionResult AddMonthlyIncome(string name, decimal amount, string lastTab = null)
        {
            InitializeAction(lastTab);

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            InsertRepository.Instance.AddPayment(_userUtility.User.Id, name.Trim(), amount, PaymentType.MonthlyIncome);

            Session["Message"] = _userUtility.Language.GetText(48);

            return RedirectToAction("Index", "Account");
        }

        public ActionResult Transaction(int paymentId, string lastTab = null)
        {
            InitializeAction(lastTab);

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            ViewBag.PaymentId = paymentId;
            ViewBag.PaymentName = _userUtility.User.Payments.Find(p => p.Id == paymentId).Name;

            return View();
        }

        public ActionResult AddTransaction(int paymentId, string description, decimal amount)
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            if (_userUtility.User.Payments.Any(p => p.Id == paymentId))
            {
                InsertRepository.Instance.AddTransaction(paymentId, amount, description.Trim());

                Session["Message"] = _userUtility.Language.GetText(49);
            }
            else
            {
                Session["Exception"] = _userUtility.Language.GetText(47);
            }

            return RedirectToAction("Index", "Account");
        }

        public ActionResult AddQuickTransaction(int balanceId, int paymentId, string description, decimal amount)
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            IPayment payment = _userUtility.User.Payments.Find(p => p.Id == paymentId);
            Balance balance = _userUtility.User.Balances.Find(b => b.Id == balanceId);

            if (payment != null && balance != null)
            {
                InsertRepository.Instance.AddTransaction(paymentId, amount, description.Trim());

                if (payment is MonthlyBill)
                {
                    ChangeRepository.Instance.ChangeBalance(balance.Id, balance.Name, balance.BalanceAmount - amount);
                }
                else if (payment is MonthlyIncome)
                {
                    ChangeRepository.Instance.ChangeBalance(balance.Id, balance.Name, balance.BalanceAmount + amount);
                }

                Session["Message"] = _userUtility.Language.GetText(50);
            }
            else
            {
                Session["Exception"] = _userUtility.Language.GetText(47);
            }

            return RedirectToAction("Index", "Account");
        }
    }
}