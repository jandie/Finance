using System;
using System.Globalization;
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

        private void InitializeAction()
        {
            _userUtility = SessionUtility.InitializeUtil(Session["UserUtility"]);

            Session["UserUtility"] = _userUtility;
        }

        private void SaveAction()
        {
            Session["UserUtility"] = _userUtility;
        }

        public string AddBalance(string name, string balance)
        {
            decimal balanceD = Convert.ToDecimal(balance, new CultureInfo("en-US"));
            InitializeAction();

            if (_userUtility.User == null)
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(1),
                    Success = false,
                    LogOut = true,
                    Object = null
                });

            if (!new InsertLogic().AddBankAccount(_userUtility.User, name.Trim(), 
                balanceD))
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(47),
                    Success = false,
                    Object = null
                });

            SaveAction();
            return JsonConvert.SerializeObject(new Response
            {
                Message = _userUtility.Language.GetText(44),
                Success = true,
                Object = _userUtility.User
            });
        }

        public string AddMonthlyBill(string name, string amount)
        {
            decimal amountD = Convert.ToDecimal(amount, new CultureInfo("en-US"));
            InitializeAction();

            if (_userUtility.User == null)
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(1),
                    Success = false,
                    LogOut = true,
                    Object = null
                });

            if (!new InsertLogic().AddPayment(_userUtility.User, name.Trim(),
                amountD, PaymentType.MonthlyBill))
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(47),
                    Success = false,
                    Object = null
                });

            SaveAction();
            return JsonConvert.SerializeObject(new Response
            {
                Message = _userUtility.Language.GetText(46),
                Success = true,
                Object = _userUtility.User
            });
        }

        public string AddMonthlyIncome(string name, string amount)
        {
            decimal amountD = Convert.ToDecimal(amount, new CultureInfo("en-US"));
            InitializeAction();

            if (_userUtility.User == null)
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(1),
                    Success = false,
                    LogOut = true,
                    Object = null
                });

            if (!new InsertLogic().AddPayment(_userUtility.User, name.Trim(),
                amountD, PaymentType.MonthlyIncome))
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(47),
                    Success = false,
                    Object = null
                });

            SaveAction();
            return JsonConvert.SerializeObject(new Response
            {
                Message = _userUtility.Language.GetText(48),
                Success = true,
                Object = _userUtility.User
            });
        }

        public string AddTransaction(int paymentId, string description, string amount,
            int balanceId)
        {
            decimal amountD = Convert.ToDecimal(amount, new CultureInfo("en-US"));
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
                balanceId, amountD, description.Trim()))
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
    }
}