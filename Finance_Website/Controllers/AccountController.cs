using System.Web.Mvc;
using Finance_Website.Models.Utilities;
using Repository;
using Repository.Exceptions;

namespace Finance_Website.Controllers
{
    public class AccountController : Controller
    {
        private UserUtility _userUtility;

        public void InitializeAction(string lastTab = null)
        {
            _userUtility = Session["UserUtility"] as UserUtility ?? new UserUtility();

            _userUtility.Refresh(lastTab);

            Session["UserUtility"] = _userUtility;
        }

        public void LoginAction(string password, string email)
        {
            _userUtility = new UserUtility
            {
                Email = email,
                Password = password
            };

            _userUtility.Refresh();

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
            ViewBag.Languages = DataRepository.Instance.LoadLanguages();
            ViewBag.Currencies = DataRepository.Instance.LoadCurrencies();

            return View();
        }

        [HttpPost]
        public ActionResult MyAccount(string name, string lastName, int currencyId, int languageId,
            string currentPassword, string password = "", string password2 = "")
        {

            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");
            
            ViewBag.Languages = DataRepository.Instance.LoadLanguages();
            ViewBag.Currencies = DataRepository.Instance.LoadCurrencies();

            try
            {
                ChangeRepository.Instance.ChangeUser(name, lastName, _userUtility.User.Email, currencyId,
                languageId, currentPassword, password, password2, _userUtility.Language);

                if (DataRepository.Instance.Login(_userUtility.User.Email, password, false, false, false) != null)
                {
                    _userUtility.Password = password;

                    Session["UserUtility"] = _userUtility;
                }

                InitializeAction();

                Session["Message"] = _userUtility.Language.GetText(76);
            }
            catch (UserChangeException ex)
            {
                Session["Exception"] = ex.Message;
            }

            return View();
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
            ViewBag.Languages = DataRepository.Instance.LoadLanguages();
            ViewBag.Currencies = DataRepository.Instance.LoadCurrencies();

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
            ViewBag.Languages = DataRepository.Instance.LoadLanguages();
            ViewBag.Currencies = DataRepository.Instance.LoadCurrencies();

            try
            {
                _userUtility.User = DataRepository.Instance.CreateUser(name, lastName, email, password, password2,
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