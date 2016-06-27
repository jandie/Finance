using System;
using System.Web.Mvc;
using Finance_Website.Models.Utilities;
using Library.Classes;
using Library.Exceptions;
using Repository;

namespace Finance_Website.Controllers
{
    public class AccountController : Controller
    {
        private User _user;

        public void InitializeAction(string lastTab = null)
        {
            if (string.IsNullOrWhiteSpace(Session["LastTab"] as string))
                Session["LastTab"] = lastTab;

            _user = null;

            _user = Session["User"] as User;

            if (_user == null) throw new WrongUsernameOrPasswordException("Not logged in.");

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
                Session["Exception"] = "Kon gegevens niet laden";
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
                    Session["Exception"] = "Username or password are not correct";

                    return RedirectToAction("Login");
                }

                Session["User"] = _user;

                Session["Password"] = password;

                Session["Message"] = "User logged in";

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

            Session["Message"] = "User logged out";

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
                Session["Exception"] = "Name is required";

            else if (string.IsNullOrWhiteSpace(lastName))
                Session["Exception"] = "Lastname is required";

            else if (string.IsNullOrWhiteSpace(email))
                Session["Exception"] = "Email is required";

            else if (!RegexUtilities.Instance.IsValidEmail(email))
                Session["Exception"] = "Email has to be valid";

            else if (password.Length < 8)
                Session["Exception"] = "Password needs to be at least 8 characters long";

            else if (password.Contains(" "))
                Session["Exception"] = "Password may not contain spaces";

            else if (password != password2)
                Session["Exception"] = "The two passwords must be the same";

            else
            {
                User user = DataRepository.Instance.CreateUser(name, lastName, email, password);

                if (user == null)
                {
                    Session["Exception"] = "User registration failed";

                    return View();
                }

                Session["User"] = user;

                Session["Password"] = password;

                Session["Message"] = "User registered";

                return RedirectToAction("Index", "Account");
            }

            return View();
        }
    }
}