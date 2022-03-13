using System.Web.Mvc;

namespace Flight_Management_System.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("SignIn","Auth");
        }
    }
}