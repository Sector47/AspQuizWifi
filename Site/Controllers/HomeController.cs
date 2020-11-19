using Site.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdminMobileQuizOverWifi;

namespace Site.Controllers
{
    public class HomeController : Controller
    {
        // connection to DB

        private MobileQuizEntities db = new MobileQuizEntities();

        public ActionResult Index()
        {
            ViewBag.ID = "You are logged in with sessionid: " + HttpContext.Session.SessionID;
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

        public ActionResult LogIn()
        {
            ViewBag.ID = "You are logged in with sessionid: " + HttpContext.Session.SessionID;
            Session["started"] = "true";

            // Heidi's login stuff, can be removed once Johnathan's is added
            // sends all the logins to the form

            List<LOGIN> result = new List<LOGIN>();
            var allLogins = db.LOGINs.ToList();
            result = allLogins;


            return View(result);
        }
        
        public ActionResult LogOut()
        {
            // TODO Call sql request to remove sessionId from logged in user
            Session.Clear();
            Session.Abandon();
            return View("Index");
        }


        // LOGIN 
        [HttpPost]
        public ActionResult LogInAttempt(string username, string password)
        {
            string session = HttpContext.Session.SessionID;
            // Create our asp database object to send through login attempt
            DatabaseCreator databaseCreator = new DatabaseCreator();
            // attempt to login with passed through data.
            bool loginStatus = databaseCreator.loginDB(username, password, session);
            if (loginStatus)
            {

                //HttpContext.Session.SetString("LoggedIn", "true");
                ViewBag.ID = "You are logged in as " + username + " with sessionid: " + HttpContext.Session.SessionID;
                ViewData["Password"] = password;
                // Send them back to the home page logged in
                // TODO Create a cookie instead of the viewdata password part
                return View("Index");
            }
            // if not logged in, return to the login view with viewdata of an error
            // TODO Parse the type of error based on the sql return value.
            ViewBag.Error = "There was an error";
            // Heidi's login stuff, can be removed once Johnathan's is added
            // sends all the logins to the form

            List<LOGIN> result = new List<LOGIN>();
            var allLogins = db.LOGINs.ToList();
            result = allLogins;
            return View("LogIn");
        }
    }
}