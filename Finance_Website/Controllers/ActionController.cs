using System.Web.Mvc;

namespace Finance_Website.Controllers
{
    public class ActionController : Controller
    {
        public ActionResult AddBalance(string name, int balance)
        {
            return RedirectToAction("Index", "Account");
        }
    }
}