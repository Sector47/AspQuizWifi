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
            return View();
        }

        public ActionResult Quizzes()
        {
            if (System.Web.HttpContext.Current.Session["UserID"] != null && System.Web.HttpContext.Current.Session["UserID"].ToString() != "")
            {
                DatabaseCreator databaseCreator = new DatabaseCreator();

                return View("Quizzes", databaseCreator.getQuizzes(HttpContext.Session["User_ID"].ToString()));
            }

            ViewData["Title"] = "You must log in first in order to view the quizzes page";
            return View("LogIn");
        }

        public ActionResult Account()
        {
            if(System.Web.HttpContext.Current.Session["UserID"] != null && System.Web.HttpContext.Current.Session["UserID"].ToString() != "")
            {
                ViewBag.Message = "Your contact page.";
                return View();
            }

            ViewData["Title"] = "You must log in first in order to view the account page";
            return View("LogIn");  
        }

        public ActionResult LogIn()
        {
            ViewData["Title"] = "Log In";
            return View();
        }
        
        public ActionResult LogOut()
        {
            DatabaseCreator databaseCreator = new DatabaseCreator();
            databaseCreator.removeSesssionDB(HttpContext.Session.SessionID);
            // remove sessionId and all cookies from logged in user
            Session.Clear();
            Session.Abandon();
            return View("Index");
        }

        public ActionResult GoToQuiz(int quizID)
        {
            return View("Quiz", quizID);
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

            // If login attempt was successful, display message that they are logged in. Send them back to the home page
            if (loginStatus)
            {
                // Set the session id to an accessible userid as well as a cookie for the name
                HttpContext.Session["UserID"] = HttpContext.Session.SessionID;
                HttpContext.Session["UserName"] = username;
                HttpContext.Session["User_ID"] = 11;

                // Send them back to the home page logged in
                return View("Index");
            }
            // if not logged in, return to the login view with viewdata of an error
            // TODO Parse the type of error based on the sql return value.
            ViewBag.Error = "There was an error";
            return View("LogIn");
        }

        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            DatabaseCreator databaseCreator = new DatabaseCreator();

            if (currentPassword != "" && databaseCreator.CheckPasswordDB(currentPassword))
            {
                ViewBag.PasswordChange = "Your current password was incorrect.";
                return View("Account");
            }
            else if(currentPassword == "")
            {
                ViewBag.PasswordChange = "Current password cannot be blank";
                return View("Account");
            }
            else 
            {
                if(newPassword != "" && confirmNewPassword != "")
                {
                    if (newPassword == confirmNewPassword)
                    {
                        if(newPassword != currentPassword)
                        {
                            databaseCreator.UpdatePasswordDB(HttpContext.Session["UserName"].ToString(), newPassword);
                            ViewBag.PasswordChange = "Your password was successfully changed.";
                        }
                        else
                        {
                            ViewBag.PasswordChange = "Your new password cannot be the same as your current password.";
                            return View("Account");
                        }                                            
                    }
                    else
                    {
                        ViewBag.PasswordChange = "Your new password and confirmation did not match.";
                        return View("Account");
                    }
                }
                else
                {
                    ViewBag.PasswordChange = "Your new password or confirmation cannot be blank.";
                    return View("Account");
                }              
            }

            return View("Account");
        }
    }
}