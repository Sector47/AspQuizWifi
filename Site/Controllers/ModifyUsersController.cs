using AdminMobileQuizOverWifi;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class ModifyUsersController : Controller
    {
        // DB Controller
        private MobileQuizWifiDBEntities db = new MobileQuizWifiDBEntities();

        // GET: ModifyUsers
        public ActionResult Index()
        {
            if (isInstructor())
            {
                var users = db.USERS;
                return View(users.ToList());
            }
            ViewData["Title"] = "You must be logged in as an instructor to view the Modify Users page";
            return View("../Home/LogIn");
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        // Check if the current user is an instructor.
        public bool isInstructor()
        {
            DatabaseCreator databaseCreator = new DatabaseCreator();
            return databaseCreator.isInstructorDB(getLoggedInUserID());
        }

        // Gets logged in user's ID
        public int getLoggedInUserID()
        {
            DatabaseCreator databaseCreator = new DatabaseCreator();
            return databaseCreator.getUserIDDB(HttpContext.Session["UserName"].ToString());
        }
    }
}