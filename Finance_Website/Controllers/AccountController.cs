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
            if (!UserUtility.UserIsValid(Session["User"] as User))
                return RedirectToAction("Login", "Account");

            try
            {
                User user = Session["User"] as User;

                if (user == null) return RedirectToAction("Login", "Account");

                user = DataRepository.Instance.Login(user.Email, Session["Password"] as string, true, true, true);

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

                Session["User"] = user;

                Session["Password"] = password;

                Session["Message"] = "Gebruiker is ingelogd!";

                return RedirectToAction("Index", "Account");
            }
            catch (WrongUsernameOrPasswordException ex)
            {
                Session["Exception"] = ex.Message;

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
            Session["User"] = null;

            Session["Message"] = "Gebruiker is uitgelogd!";

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
                Session["Exception"] = "Naam moet ingevuld zijn.";

            else if (string.IsNullOrWhiteSpace(lastName))
                Session["Exception"] = "Achternaam moet ingevuld zijn.";

            else if (string.IsNullOrWhiteSpace(email))
                Session["Exception"] = "Email moet ingevuld zijn.";

            else if (!RegexUtilities.Instance.IsValidEmail(email))
                Session["Exception"] = "Email moet een geldig email adres zijn.";

            else if (password.Length < 8)
                Session["Exception"] = "Het wachtwoord moet een minimale lengte van 8 hebben.";

            else if (password.Contains(" "))
                Session["Exception"] = "Het wachtwoord mag geen spaties hebben.";

            else if (password != password2)
                Session["Exception"] = "De 2 wachtwoorden moeten gelijk zijn aan elkaar";

            else
            {
                User user = DataRepository.Instance.CreateUser(password, name, lastName, email);

                if (user == null)
                {
                    Session["Exception"] = "Gebruiker registreren is niet gelukt.";

                    return View();
                }

                Session["User"] = user;

                Session["Password"] = password;

                Session["Message"] = "Gebruiker is geregistreerd!";

                return RedirectToAction("Index", "Account");
            }

            return View();
        }

    }
}