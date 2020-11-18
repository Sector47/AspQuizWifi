using Site.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class HomeController : Controller
    {
        // connection to DB

        private MobileQuizEntities db = new MobileQuizEntities();

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Quizzes()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Account()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        // LOGIN 
        public ActionResult LogIn(string user, string pw)
        {
            // Heidi's login stuff, can be removed once Johnathan's is added
            // sends all the logins to the form
           
            List<LOGIN> result = new List<LOGIN>();
            var allLogins = db.LOGINs.ToList();
            result = allLogins;
            

            return View(result);
        }
    }
}