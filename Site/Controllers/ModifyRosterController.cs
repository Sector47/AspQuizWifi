using AdminMobileQuizOverWifi;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class ModifyRosterController : Controller
    {
        private Models.DBEntities db = new DBEntities();
        // not instructor msg
        string notInstructor = "You must be logged in as an instructor to view this page.";
        // not logged in msg
        string notLoggedIn = "You are not logged in and do not have the correct permissions to view this page.";

        // GET: ModifyRoster
        public ActionResult Index()
        {
            if (loggedIn())
            {
                if (isInstructor())
                {
                    var rosters = db.ROSTERs;
                    return View(rosters.ToList());
                }
                else
                {
                    ViewBag.NotPermitted = notInstructor;
                    return View();
                }
            }
            else
            {
                ViewBag.NotPermitted = notLoggedIn;
                return View();
            }
            return View();
        }

        // Login Check Stuff
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

        // Checks if user is logged in
        public bool loggedIn()
        {
            if (HttpContext.Session["UserSessionID"] != null && HttpContext.Session["UserSessionID"].ToString() != "")
            {
                return true;
            }
            else
                return false;
        }
    }
}