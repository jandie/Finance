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
            User user = Session["user"] as User;

            if (user != null)
            {
                ViewBag.User = DataRepository.Instance.Login(user.Email, Session["pass"] as string, true, true, true);

                Session["user"] = ViewBag.User;

                return View();
            }

            return RedirectToAction("Login", "Account");
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

                Session["user"] = user;

                Session["pass"] = password;

                Session["message"] = "Gebruiker is ingelogd!";

                return RedirectToAction("Index", "Account");
            }
            catch (WrongUsernameOrPasswordException ex)
            {
                Session["badError"] = ex.Message;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return View();
            }
        }

        public ActionResult Loguit()
        {
            Session["user"] = null;

            Session["message"] = "Gebruiker is uitgelogd!";

            return RedirectToAction("Index", "Home");
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
            {
                Session["badError"] = "Naam moet ingevuld zijn.";
            }
            else if (string.IsNullOrWhiteSpace(lastName))
            {
                Session["badError"] = "Achternaam moet ingevuld zijn.";
            }
            else if (string.IsNullOrWhiteSpace(email))
            {
                Session["badError"] = "Email moet ingevuld zijn.";
            }
            else if (!RegexUtilities.Instance.IsValidEmail(email))
            {
                Session["badError"] = "Email moet een geldig email adres zijn.";
            }
            else if (password.Length < 8)
            {
                Session["badError"] = "Het wachtwoord moet een minimale lengte van 8 hebben.";
            }
            else if (password.Contains(" "))
            {
                Session["badError"] = "Het wachtwoord mag geen spaties hebben.";
            }
            else if (password != password2)
            {
                Session["badError"] = "De 2 wachtwoorden moeten gelijk zijn aan elkaar";
            }
            else
            {
                User user = DataRepository.Instance.CreateUser(password, name, lastName, email);

                if (user != null)
                {
                    Session["user"] = user;

                    Session["pass"] = password;

                    Session["message"] = "Gebruiker is geregistreerd!";

                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

    }
}