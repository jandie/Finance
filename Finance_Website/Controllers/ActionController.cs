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
        public ActionResult AddBalance(string name, decimal balance)
        {
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            try
            {
                InsertRepository.Instance.AddBankAccount(user.Id, name, balance);

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
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            Session["LastTab"] = lastTab;

            try
            {
                InsertRepository.Instance.AddPayment(user.Id, name, amount, PaymentType.MonthlyBill);

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
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            Session["LastTab"] = lastTab;

            try
            {
                InsertRepository.Instance.AddPayment(user.Id, name, amount, PaymentType.MonthlyIncome);

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
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            Session["LastTab"] = lastTab;

            try
            {
                ViewBag.PaymentId = paymentId;
                ViewBag.PaymentName = user.Payments.Find(p => p.Id == paymentId).Name;
            }
            catch (Exception)
            {
                Session["Exception"] = "Action couldn't be completed.";
            }

            return View();
        }

        public ActionResult AddTransaction(int paymentId, string description, decimal amount)
        {
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            try
            {
                if (user.Payments.Any(p => p.Id == paymentId))
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
            User user = DataRepository.Instance.Login((Session["User"] as User)?.Email, Session["Password"] as string,
                true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            try
            {
                IPayment payment = user.Payments.Find(p => p.Id == paymentId);
                Balance balance = user.Balances.Find(b => b.Id == balanceId);

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