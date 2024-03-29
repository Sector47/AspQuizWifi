﻿
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
                // user is logged in
                if (isInstructor())
                {
                    // user is instructor, so return the view w/ model
                    var rosters = db.ROSTERs.Include(r => r.COURSE).Include(r => r.USER);
                    rosters = rosters.OrderBy(r => r.COURSE.COU_NAME);
                    return View(rosters.ToList());
                }
                else
                {
                    // user is student, send proper viewbag msg
                    ViewBag.NotPermitted = notInstructor;
                    return View();
                }
            }
            else
            {
                // user is not logged in, send proper viewbag msg
                ViewBag.NotPermitted = notLoggedIn;
                return View();
            }
        }

        public ActionResult Create()
        {
            if (loggedIn())
            {
                 // user is logged in
                if (isInstructor())
                {
                    // user is instructor, indicate so in the viewbag
                    ViewBag.Permission = true;
                    ViewBag.COURSE_ID = new SelectList(db.COURSEs, "COURSE_ID", "COU_NAME");
                    ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USERNAME");
                  
                    return View();
                }
                else
                {
                    // user is student, send proper viewbag msg, indicate not permitted
                    ViewBag.Permission = false;
                    ViewBag.PermissionMsg = notInstructor;
                    return View();
                }
            }
            else
            {
                // user is not logged in, send proper viewbag msg, indicate not permitted
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
                try
                {
                    // attempt to create the row in the db
                    db.ROSTERs.Add(roster);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    // if fails, create viewbag msg
                    ViewBag.Msg = "Unable to add. The student is already in this course.";
                   
                }
            }

            // return view with roster
            ViewBag.COURSE_ID = new SelectList(db.COURSEs, "COURSE_ID", "COU_NAME", roster.COURSE_ID);
            ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USERNAME", roster.USER_ID);
            return View(roster);
        }


        public ActionResult Delete(int? id)
        {
            if (loggedIn())
            {
                // user is logged in
                if (isInstructor())
                {
                    // user is instructor
                    if (id == null)
                    {
                        // id was not sent in action result, bad request
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    // find the roster listing using the id that came in
                    ROSTER roster = db.ROSTERs.Find(id);
                    if (roster == null)
                    {
                        // if that roster doesnt exist, error
                        return HttpNotFound();
                    }

                    // return the view with the correct roster listing
                    ViewBag.COURSE_ID = new SelectList(db.COURSEs, "COURSE_ID", "COU_NAME", roster.COURSE_ID);
                    ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USERNAME", roster.USER_ID);
                    return View(roster);
                }
                else
                {
                    // not user, return viewbag msg that says so 
                    ViewBag.PermissionMsg = notInstructor;
                    return View();
                }
            }
            else
            {
                // not logged in, return viewbag msg that says so 
                ViewBag.PermissionMsg = notLoggedIn;
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // delete the roster that came in 
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