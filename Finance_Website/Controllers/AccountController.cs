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
        public ActionResult Index()
        {
            User user = Session["User"] as User;

            user = DataRepository.Instance.Login(user.Email, Session["Password"] as string, true, true, true);

            if (user == null)
                return RedirectToAction("Login", "Account");

            try
            {
                ViewBag.User = user;
                Session["User"] = user;
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
                User user = DataRepository.Instance.Login(email, password, false, false, false);

                if (user == null)
                {
                    Session["Exception"] = "Username or password are not correct";

                    return RedirectToAction("Login");
                }

                Session["User"] = user;

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
                User user = DataRepository.Instance.CreateUser(password, name, lastName, email);

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