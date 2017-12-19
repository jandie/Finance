using System.Web.Mvc;
using FinanceLibrary.Logic;
using Finance_Website.Models;
using Finance_Website.Models.Utilities;
using Newtonsoft.Json;

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

        [HttpPost]
        public string CheckSession(string email)
        {
            InitializeAction();

            if (_userUtility.User == null)
                return JsonConvert.SerializeObject(new Response
                {
                    Message = "Session had ended",
                    Success = false,
                    LogOut = true,
                    Object = null
                });

            return JsonConvert.SerializeObject(new Response
            {
                Message = "Session is still active",
                Success = true,
                LogOut = false,
                Object = null
            });
        }

        #region Balance
        [HttpPost]
        public string ChangeBalance(int id, string name, decimal balanceAmount)
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

            if (new ChangeLogic().ChangeBalance(_userUtility.User, id, name,
                    balanceAmount))
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(51),
                    Success = true,
                    LogOut = false,
                    Object = _userUtility.User
                });

            return JsonConvert.SerializeObject(new Response
            {
                Message = _userUtility.Language.GetText(47),
                Success = false,
                LogOut = false,
                Object = _userUtility.User
            });
        }

        [HttpPost]
        public string DeleteBalance(int id)
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

            if (new DeleteLogic().DeleteBalance(_userUtility.User, id))
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(52),
                    Success = true,
                    LogOut = false,
                    Object = _userUtility.User
                });

            return JsonConvert.SerializeObject(new Response
            {
                Message = _userUtility.Language.GetText(47),
                Success = false,
                LogOut = false,
                Object = _userUtility.User
            });
        }

        #endregion

        #region Payment

        [HttpPost]
        public string ChangePayment(int id, string name, decimal amount)
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

            if (new ChangeLogic().ChangePayment(_userUtility.User, id, name, amount))
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(53),
                    Success = true,
                    LogOut = false,
                    Object = _userUtility.User
                });

            return JsonConvert.SerializeObject(new Response
            {
                Message = _userUtility.Language.GetText(47),
                Success = false,
                LogOut = false,
                Object = _userUtility.User
            });
        }

        [HttpPost]
        public string DeletePayment(int id, string lastTab = null)
        {
            InitializeAction(lastTab);

            if (_userUtility.User == null)
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(1),
                    Success = false,
                    LogOut = true,
                    Object = null
                });

            if (new DeleteLogic().DeletePayment(_userUtility.User, id))
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(54),
                    Success = true,
                    LogOut = false,
                    Object = _userUtility.User
                });

            return JsonConvert.SerializeObject(new Response
            {
                Message = _userUtility.Language.GetText(47),
                Success = false,
                LogOut = false,
                Object = _userUtility.User
            });
        }

        #endregion Payment

        #region Transaction

        [HttpPost]
        public string ChangeTransaction(int id, decimal amount, string description)
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

            if (new ChangeLogic().ChangeTransaction(_userUtility.User, id, amount, description))
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(55),
                    Success = true,
                    LogOut = false,
                    Object = _userUtility.User
                });

            return JsonConvert.SerializeObject(new Response
            {
                Message = _userUtility.Language.GetText(47),
                Success = false,
                LogOut = false,
                Object = _userUtility.User
            });
        }

        [HttpPost]
        public string DeleteTransaction(int id)
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

            if (new DeleteLogic().DeleteTransaction(_userUtility.User, id))
                return JsonConvert.SerializeObject(new Response
                {
                    Message = _userUtility.Language.GetText(56),
                    Success = true,
                    LogOut = false,
                    Object = _userUtility.User
                });

            return JsonConvert.SerializeObject(new Response
            {
                Message = _userUtility.Language.GetText(47),
                Success = false,
                LogOut = false,
                Object = _userUtility.User
            });
        }

        #endregion Transaction
    }
}