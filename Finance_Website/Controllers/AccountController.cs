using System;
using System.Web.Mvc;
using Finance_Website.Models.Utilities;
using Library.Classes;
using Library.Classes.Language;
using Library.Exceptions;
using Repository;

namespace Finance_Website.Controllers
{
    public class AccountController : Controller
    {
        private User _user;
        private Language _language;

        public void InitializeAction(string lastTab = null)
        {
            if (string.IsNullOrWhiteSpace(Session["LastTab"] as string))
                Session["LastTab"] = lastTab;

            _user = null;

            _user = Session["User"] as User;

            if (_user == null)
            {
                if (!(Session["Language"] is Language)) _language = LanguageRepository.Instance.LoadLanguage(0);

                throw new WrongUsernameOrPasswordException(_language.GetText(31));
            }

            if (!(Session["Language"] is Language)) _language = LanguageRepository.Instance.LoadLanguage(0);

            _user = DataRepository.Instance.Login(_user.Email, Session["Password"] as string, true, true, true);
        }

        public ActionResult Index()
        {
            try
            {
                InitializeAction();
            }
            catch (WrongUsernameOrPasswordException x)
            {
                Session["Exception"] = x.Message;

                return RedirectToAction("Login", "Account");
            }

            try
            {
                ViewBag.User = _user;
                Session["User"] = _user;
            }
            catch (Exception)
            {
                Session["Exception"] = _language.GetText(32);
            }

            return View();
        }

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            try
            {
                _user = DataRepository.Instance.Login(email, password, false, false, false);

                if (_user == null)
                {
                    Session["Exception"] = _language.GetText(33);

                    return RedirectToAction("Login");
                }

                Session["User"] = _user;

                Session["Password"] = password;

                Session["Message"] = _language.GetText(57);

                return RedirectToAction("Index", "Account");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return View();
        }

        public ActionResult Loguit()
        {
            Session["User"] = null;

            Session["Message"] = _language.GetText(34);

            return RedirectToAction("Login", "Account");
        }

        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string name, string lastName, string email, string password, string password2)
        {
            ViewBag.Name = name;
            ViewBag.LastName = lastName;
            ViewBag.Email = email;

            if (string.IsNullOrWhiteSpace(name))
                Session["Exception"] = _language.GetText(35);

            else if (string.IsNullOrWhiteSpace(lastName))
                Session["Exception"] = _language.GetText(36);

            else if (string.IsNullOrWhiteSpace(email))
                Session["Exception"] = _language.GetText(37);

            else if (!RegexUtilities.Instance.IsValidEmail(email))
                Session["Exception"] = _language.GetText(38);

            else if (password.Length < 8)
                Session["Exception"] = _language.GetText(39);

            else if (password.Contains(" "))
                Session["Exception"] = _language.GetText(40);

            else if (password != password2)
                Session["Exception"] = _language.GetText(41);

            else
            {
                User user = DataRepository.Instance.CreateUser(name, lastName, email, password);

                if (user == null)
                {
                    Session["Exception"] = _language.GetText(42);

                    return View();
                }

                Session["User"] = user;

                Session["Password"] = password;

                Session["Message"] = _language.GetText(43);

                return RedirectToAction("Index", "Account");
            }

            return View();
        }
    }
}