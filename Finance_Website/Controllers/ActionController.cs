using System;
using System.Web.Mvc;
using Finance_Website.Models.Utilities;
using Library.Classes;
using Library.Enums;
using Repository;

namespace Finance_Website.Controllers
{
    public class ActionController : Controller
    {
        public ActionResult AddBalance(string name, decimal balance)
        {
            if (!UserUtility.UserIsValid(Session["User"] as User))
                return RedirectToAction("Login", "Account");

            try
            {
                User user = Session["User"] as User;

                InsertRepository.Instance.AddBankAccount(user.Id, name, balance);

                Session["Message"] = "Bank account was added successfully.";
            }
            catch (Exception)
            {
                Session["Exception"] = "BalanceAmount couldn't be added";
            }

            return RedirectToAction("Index", "Account");
        }

        public ActionResult AddMonthlyBill(string name, decimal amount)
        {
            if (!UserUtility.UserIsValid(Session["User"] as User))
                return RedirectToAction("Login", "Account");

            try
            {
                User user = Session["User"] as User;

                InsertRepository.Instance.AddPayment(user.Id, name, amount, PaymentType.MonthlyBill);

                Session["Message"] = "Monthly bill was added successfully.";
            }
            catch (Exception)
            {
                Session["Exception"] = "Action couldn't be completed.";
            }

            return RedirectToAction("Index", "Account");
        }

        public ActionResult AddMonthlyIncome(string name, decimal amount)
        {
            if (!UserUtility.UserIsValid(Session["User"] as User))
                return RedirectToAction("Login", "Account");

            try
            {
                User user = Session["User"] as User;

                InsertRepository.Instance.AddPayment(user.Id, name, amount, PaymentType.MonthlyIncome);

                Session["Message"] = "Monthly income was added successfully.";
            }
            catch (Exception)
            {
                Session["Exception"] = "Action couldn't be completed.";
            }

            return RedirectToAction("Index", "Account");
        }
    }
}