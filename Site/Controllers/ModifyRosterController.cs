
using AdminMobileQuizOverWifi;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace Site.Controllers
{
    public class ModifyRosterController : Controller
    {
        private DBEntities db = new DBEntities();
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
                    var rosters = db.ROSTERs.Include(r => r.USER).Include(r => r.COURSE);
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

        public ActionResult Create()
        {
            if (loggedIn())
            {
                if (isInstructor())
                {
                    ViewBag.Permission = true;
                    ViewBag.COURSE_ID = new SelectList(db.COURSEs, "COURSE_ID", "COU_NAME");
                    ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USERNAME");
                    return View();
                }
                else
                {
                    ViewBag.Permission = false;
                    ViewBag.PermissionMsg = notInstructor;
                    return View();
                }
            }
            else
            {
                ViewBag.Permission = false;
                ViewBag.PermissionMsg = notLoggedIn;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "COURSE_ID, USER_ID")] ROSTER roster)
        {
            if (ModelState.IsValid)
            {
                db.ROSTERs.Add(roster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.COURSE_ID = new SelectList(db.COURSEs, "COURSE_ID", "COU_NAME", roster.COURSE_ID);
            ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USERNAME", roster.USER_ID);
            return View(roster);
        }


        public ActionResult Delete(int? id)
        {
            if (loggedIn())
            {
                if (isInstructor())
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    ROSTER roster = db.ROSTERs.Find(id);
                    if (roster == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.COURSE_ID = new SelectList(db.COURSEs, "COURSE_ID", "COU_NAME", roster.COURSE_ID);
                    ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USERNAME", roster.USER_ID);
                    return View(roster);
                }
                else
                {
                    ViewBag.PermissionMsg = notInstructor;
                    return View();
                }
            }
            else
            {
                ViewBag.PermissionMsg = notLoggedIn;
                return View();
            }

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ROSTER roster = db.ROSTERs.Find(id);
            db.ROSTERs.Remove(roster);
            db.SaveChanges();
            return RedirectToAction("Index");
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