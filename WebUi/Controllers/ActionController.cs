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

        public string AddBalance(string name, decimal balance)
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

            if (!new InsertLogic().AddBankAccount(_userUtility.User, name.Trim(), 
                balance))
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

        public string AddMonthlyBill(string name, decimal amount)
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

            if (!new InsertLogic().AddPayment(_userUtility.User, name.Trim(),
                amount, PaymentType.MonthlyBill))
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

        public string AddMonthlyIncome(string name, decimal amount)
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

            if (!new InsertLogic().AddPayment(_userUtility.User, name.Trim(),
                amount, PaymentType.MonthlyIncome))
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
    }
}