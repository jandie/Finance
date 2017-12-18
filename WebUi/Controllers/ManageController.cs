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

            if (new ChangeLogic().ChangePayment(_userUtility.User, id, name, amount))
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

            if (new DeleteLogic().DeletePayment(_userUtility.User, id))
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