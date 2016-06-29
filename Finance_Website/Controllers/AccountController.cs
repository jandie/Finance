using System;
using System.Web.Mvc;
using Finance_Website.Models.Utilities;
using Library.Classes;
using Library.Classes.Language;
using Repository;

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

        // GET: Account
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

        // GET: Account
        public ActionResult Register()
        {
           
            InitializeAction();

            ViewBag.Languages = DataRepository.Instance.LoadLanguages();
            ViewBag.Currencies = DataRepository.Instance.LoadCurrencies();

            return View();
        }

        [HttpPost]
        public ActionResult Register(string name, string lastName, string email, int currencyId, int languageId, string password, string password2)
        {
            
            InitializeAction();

            ViewBag.Name = name;
            ViewBag.LastName = lastName;
            ViewBag.Email = email;

            if (string.IsNullOrWhiteSpace(name))
                Session["Exception"] = _userUtility.Language.GetText(35);

            else if (string.IsNullOrWhiteSpace(lastName))
                Session["Exception"] = _userUtility.Language.GetText(36);

            else if (string.IsNullOrWhiteSpace(email))
                Session["Exception"] = _userUtility.Language.GetText(37);

            else if (!RegexUtilities.Instance.IsValidEmail(email))
                Session["Exception"] = _userUtility.Language.GetText(38);

            else if (password.Length < 8)
                Session["Exception"] = _userUtility.Language.GetText(39);

            else if (password.Contains(" "))
                Session["Exception"] = _userUtility.Language.GetText(40);

            else if (password != password2)
                Session["Exception"] = _userUtility.Language.GetText(41);

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