using System.Web.Mvc;
using FinanceLibrary.Exceptions;
using FinanceLibrary.Logic;
using Finance_Website.Models.Utilities;

namespace Finance_Website.Controllers
{
    public class AccountController : Controller
    {
        private SessionUtility _userUtility;

        public void InitializeAction(string lastTab = null)
        {
            _userUtility = SessionUtility.InitializeUtil(Session["UserUtility"], lastTab);

            Session["UserUtility"] = _userUtility;
        }

        public void LoginAction(string password, string email)
        {
            _userUtility = SessionUtility.Login(password, email);

            Session["UserUtility"] = _userUtility;
        }

        public ActionResult Index()
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            ViewBag.User = _userUtility.User;

            return View();
        }

        public ActionResult MyAccount()
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            ViewBag.User = _userUtility.User;
            ViewBag.Languages = new DataLogic().LoadLanguages();
            ViewBag.Currencies = new DataLogic().LoadCurrencies();

            return View();
        }

        [HttpPost]
        public ActionResult MyAccount(string name, string lastName, int currencyId, int languageId,
            string currentPassword, string password = "", string password2 = "")
        {
            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Languages = new DataLogic().LoadLanguages();
            ViewBag.Currencies = new DataLogic().LoadCurrencies();

            try
            {
                new ChangeLogic().ChangeUser(_userUtility.User, name, lastName, _userUtility.User.Email, currencyId,
                    languageId, currentPassword, password, password2, _userUtility.Language);

                Session["UserUtility"] = null;

                Session["Message"] = _userUtility.Language.GetText(76);
            }
            catch (ChangeUserException ex)
            {
                Session["Exception"] = ex.Message;
            }

            return RedirectToAction("Index", "Account");
        }

        public ActionResult Login()
        {
            InitializeAction();

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            LoginAction(password, email);

            if (_userUtility.User == null)
            {
                Session["Exception"] = _userUtility.Language?.GetText(33);

                return RedirectToAction("Login");
            }

            Session["Message"] = _userUtility.Language?.GetText(57);

            return RedirectToAction("Index", "Account");
        }

        public ActionResult Loguit()
        {
            Session.Clear();

            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register(string alphaKey = "")
        {
            InitializeAction();

            ViewBag.AlphaKey = alphaKey;
            ViewBag.Languages = new DataLogic().LoadLanguages();
            ViewBag.Currencies = new DataLogic().LoadCurrencies();

            return View();
        }

        [HttpPost]
        public ActionResult Register(string name, string lastName, string email, int currencyId, int languageId,
            string password, string password2, string alphaKey)
        {
            InitializeAction();

            ViewBag.Name = name;
            ViewBag.LastName = lastName;
            ViewBag.Email = email;
            ViewBag.AlphaKey = alphaKey;
            ViewBag.Languages = new DataLogic().LoadLanguages();
            ViewBag.Currencies = new DataLogic().LoadCurrencies();

            try
            {
                _userUtility.User = new DataLogic().CreateUser(name, lastName, email, password, password2,
                    currencyId, languageId, _userUtility.Language, alphaKey);

                LoginAction(password, email);

                Session["Message"] = _userUtility.Language.GetText(43);

                return RedirectToAction("Index");
            }
            catch (RegistrationException ex)
            {
                Session["Exception"] = ex.Message;
            }

            return View();
        }
    }
}