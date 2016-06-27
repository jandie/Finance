using System;
using System.Linq;
using System.Web.Mvc;
using Library.Classes;
using Library.Classes.Language;
using Library.Enums;
using Library.Exceptions;
using Library.Interfaces;
using Repository;

namespace Finance_Website.Controllers
{
    public class ActionController : Controller
    {
        private User _user;
        private Language _language;

        public void InitializeAction(string lastTab = null)
        {
            if (string.IsNullOrWhiteSpace(Session["LastTab"] as string))
                Session["LastTab"] = lastTab;

            _user = null;

            _user = Session["User"] as User;

            if (_user == null)
            {
                if (!(Session["Language"] is Language)) _language = LanguageRepository.Instance.LoadLanguage(0);

                throw new WrongUsernameOrPasswordException(_language.GetText(31));
            }

            if (!(Session["Language"] is Language)) _language = LanguageRepository.Instance.LoadLanguage(0);

            _user = DataRepository.Instance.Login(_user.Email, Session["Password"] as string, true, true, true);
        }

        public ActionResult AddBalance(string name, decimal balance, string lastTab = null)
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

            Session["LastTab"] = lastTab;

            try
            {
                InsertRepository.Instance.AddBankAccount(_user.Id, name, balance);

                Session["Message"] = _language.GetText(44);
            }
            catch (Exception)
            {
                Session["Exception"] = _language.GetText(45);
            }

            return RedirectToAction("Index", "Account");
        }

        public ActionResult AddMonthlyBill(string name, decimal amount, string lastTab = null)
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
                InsertRepository.Instance.AddPayment(_user.Id, name, amount, PaymentType.MonthlyBill);

                Session["Message"] = _language.GetText(46);
            }
            catch (Exception)
            {
                Session["Exception"] = _language.GetText(47);
            }

            return RedirectToAction("Index", "Account");
        }

        public ActionResult AddMonthlyIncome(string name, decimal amount, string lastTab = null)
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
                InsertRepository.Instance.AddPayment(_user.Id, name, amount, PaymentType.MonthlyIncome);

                Session["Message"] = _language.GetText(48);
            }
            catch (Exception)
            {
                Session["Exception"] = _language.GetText(47);
            }

            return RedirectToAction("Index", "Account");
        }

        public ActionResult Transaction(int paymentId, string lastTab = null)
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
                ViewBag.PaymentId = paymentId;
                ViewBag.PaymentName = _user.Payments.Find(p => p.Id == paymentId).Name;
            }
            catch (Exception)
            {
                Session["Exception"] = _language.GetText(47);
            }

            return View();
        }

        public ActionResult AddTransaction(int paymentId, string description, decimal amount)
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
                if (_user.Payments.Any(p => p.Id == paymentId))
                {
                    InsertRepository.Instance.AddTransaction(paymentId, amount, description);

                    Session["Message"] = _language.GetText(49);
                }
                else
                {
                    Session["Exception"] = _language.GetText(47);
                }
            }
            catch (Exception)
            {
                Session["Exception"] = _language.GetText(47);
            }

            return RedirectToAction("Index", "Account");
        }

        public ActionResult AddQuickTransaction(int balanceId, int paymentId, string description, decimal amount)
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
                IPayment payment = _user.Payments.Find(p => p.Id == paymentId);
                Balance balance = _user.Balances.Find(b => b.Id == balanceId);

                if (payment != null && balance != null)
                {
                    InsertRepository.Instance.AddTransaction(paymentId, amount, description);

                    if (payment is MonthlyBill)
                    {
                        ChangeRepository.Instance.ChangeBalance(balance.Id, balance.Name, balance.BalanceAmount - amount);
                    }
                    else if (payment is MonthlyIncome)
                    {
                        ChangeRepository.Instance.ChangeBalance(balance.Id, balance.Name, balance.BalanceAmount + amount);
                    }

                    Session["Message"] = _language.GetText(50);
                }
                else
                {
                    Session["Exception"] = _language.GetText(47);
                }
            }
            catch (Exception)
            {
                Session["Exception"] = _language.GetText(47);
            }

            return RedirectToAction("Index", "Account");
        }
    }
}