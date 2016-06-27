using System;
using System.Linq;
using System.Web.Mvc;
using Library.Classes;
using Library.Enums;
using Library.Exceptions;
using Library.Interfaces;
using Repository;

namespace Finance_Website.Controllers
{
    public class ActionController : Controller
    {
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

                Session["Message"] = "Balance was added successfully.";
            }
            catch (Exception)
            {
                Session["Exception"] = "Balance couldn't be added";
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

                Session["Message"] = "Monthly bill was added successfully.";
            }
            catch (Exception)
            {
                Session["Exception"] = "Action couldn't be completed.";
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

                Session["Message"] = "Monthly income was added successfully.";
            }
            catch (Exception)
            {
                Session["Exception"] = "Action couldn't be completed.";
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
                Session["Exception"] = "Action couldn't be completed.";
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

                    Session["Message"] = "Transaction was added successfully.";
                }
                else
                {
                    Session["Exception"] = "Transaction was not added successfully.";
                }
            }
            catch (Exception)
            {
                Session["Exception"] = "Action couldn't be completed.";
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

                    Session["Message"] = "Quick transaction was added successfully.";
                }
                else
                {
                    Session["Exception"] = "Quick transaction was not added successfully.";
                }
            }
            catch (Exception)
            {
                Session["Exception"] = "Quick transaction was not added successfully.";
            }

            return RedirectToAction("Index", "Account");
        }
    }
}