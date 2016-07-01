using System;
using System.Web.Mvc;
using Finance_Website.Models.Utilities;
using Library.Classes;
using Library.Classes.Language;
using Repository;
using static System.String;

namespace Finance_Website.Controllers
{
    public class AccountController : Controller
    {
        private UserUtility _userUtility;

        public bool InitializeAction(string lastTab = null)
        {
            bool succes = true;

            object sessionUser = Session["User"];
            object sessionPassword = Session["Password"];
            object sessionLanguage = Session["Language"];
            object sessionLastTab = Session["LastTab"];

            try
            {
                _userUtility = new UserUtility(ref sessionUser, ref sessionPassword, ref sessionLanguage, ref sessionLastTab, lastTab);
            }
            catch (Exception ex) 
            {
                succes = false;
            }

            Session["User"] = sessionUser;
            Session["Password"] = sessionPassword;
            Session["Language"] = sessionLanguage;
            Session["LastTab"] = sessionLastTab;

            return succes;
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
        public ActionResult MyAccount(string name, string lastName, int currencyId, int languageId, string currentPassword, 
            string password = "", string password2 = "")
        {

            InitializeAction();

            if (_userUtility.User == null)
                return RedirectToAction("Login", "Account");
            
            ViewBag.Languages = DataRepository.Instance.LoadLanguages();
            ViewBag.Currencies = DataRepository.Instance.LoadCurrencies();

            if (IsNullOrWhiteSpace(name))
                Session["Exception"] = _userUtility.Language.GetText(35);

            else if (IsNullOrWhiteSpace(lastName))
                Session["Exception"] = _userUtility.Language.GetText(36);

            else if (!IsNullOrWhiteSpace(password) && password.Length < 8)
                Session["Exception"] = _userUtility.Language.GetText(39);

            else if (!IsNullOrWhiteSpace(password) && password.Contains(" "))
                Session["Exception"] = _userUtility.Language.GetText(40);

            else if (DataRepository.Instance.Login(_userUtility.User.Email, currentPassword, false, false, false) == null)
                Session["Exception"] = _userUtility.Language.GetText(33);

            else if (!IsNullOrWhiteSpace(password) && password != password2)
                Session["Exception"] = _userUtility.Language.GetText(41);

            else
            {
                ChangeRepository.Instance.ChangeUser(name, lastName, _userUtility.User.Email, currencyId, languageId, currentPassword);

                if (!IsNullOrWhiteSpace(password))
                {
                    ChangeRepository.Instance.ChangePassword(_userUtility.User.Email, password, currentPassword);

                    Session["Password"] = password;
                }

                Session["Message"] = _userUtility.Language.GetText(76);

                return RedirectToAction("Index", "Account");
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
            Language language = Session["Language"] as Language;

            User user = DataRepository.Instance.Login(email, password, false, false, false);

            if (user == null)
            {
                Session["Exception"] = language.GetText(33);

                return RedirectToAction("Login");
            }

            Session["User"] = user;

            Session["Password"] = password;

            Session["Message"] = language.GetText(57);

            return RedirectToAction("Index", "Account");
        }

        public ActionResult Loguit()
        {
            InitializeAction();

            Session["User"] = null;

            Session["Message"] = _userUtility.Language.GetText(34);

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
        public ActionResult Register(string name, string lastName, string email, int currencyId, int languageId, string password, string password2, string alphaKey)
        {
            
            InitializeAction();

            ViewBag.Name = name;
            ViewBag.LastName = lastName;
            ViewBag.Email = email;
            ViewBag.AlphaKey = alphaKey;
            ViewBag.Languages = DataRepository.Instance.LoadLanguages();
            ViewBag.Currencies = DataRepository.Instance.LoadCurrencies();

            if (IsNullOrWhiteSpace(name))
                Session["Exception"] = _userUtility.Language.GetText(35);

            else if (IsNullOrWhiteSpace(lastName))
                Session["Exception"] = _userUtility.Language.GetText(36);

            else if (IsNullOrWhiteSpace(email))
                Session["Exception"] = _userUtility.Language.GetText(37);

            else if (!RegexUtilities.Instance.IsValidEmail(email))
                Session["Exception"] = _userUtility.Language.GetText(38);

            else if (password.Length < 8)
                Session["Exception"] = _userUtility.Language.GetText(39);

            else if (password.Contains(" "))
                Session["Exception"] = _userUtility.Language.GetText(40);

            else if (password != password2)
                Session["Exception"] = _userUtility.Language.GetText(41);

            else if (alphaKey != "E1j6kr!v4")
                Session["Exception"] = "Because this website is still in alpha, you need a key to be able to register.";

            else
            {
                User user = DataRepository.Instance.CreateUser(name.Trim(), lastName.Trim(), email.Trim(), password.Trim(), currencyId, languageId);

                if (user == null)
                {
                    Session["Exception"] = _userUtility.Language.GetText(42);

                    return View();
                }

                Session["User"] = user;

                Session["Password"] = password;

                Session["Message"] = _userUtility.Language.GetText(43);

                return RedirectToAction("Index", "Account");
            }

            return View();
        }
    }
}