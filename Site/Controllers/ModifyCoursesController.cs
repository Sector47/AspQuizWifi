using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Site.Models;
using AdminMobileQuizOverWifi;


namespace Site.Controllers
{
    public class ModifyCoursesController : Controller
    {
        // connection to DB Entity Framework
        private DBEntities db = new DBEntities();
        // not instructor msg
        string notInstructor = "You must be logged in as an instructor to view this page.";
        // not logged in msg
        string notLoggedIn = "You are not logged in and do not have the correct permissions to view this page.";

        // GET: ModifyCourses
        public ActionResult Index()
        {
            if (loggedIn())
            {
                if (isInstructor())
                {

                    var cOURSEs = db.COURSEs;
                    return View(db.COURSEs.ToList());
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
        }

        // GET: ManageCourses/Create
        public ActionResult Create()
        {
            if (loggedIn())
            {
                if (isInstructor())
                {
                    ViewBag.Permission = true;
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
        // POST: Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "COU_NAME, COU_SEM, COU_YEAR, COU_START_DATE, COU_END_DATE, COURSE_ID")] COURSE cOURSE)
        {
            if (ModelState.IsValid)
            {
                db.COURSEs.Add(cOURSE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cOURSE);
        }

        // GET: /Edit/5
        public ActionResult Edit(int? id)
        {
            if (loggedIn())
            {
                if (isInstructor())
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    COURSE cOURSE = db.COURSEs.Find(id);
                    if (cOURSE == null)
                    {
                        return HttpNotFound();
                    }
                    return View(cOURSE);
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

        // POST: /Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "COU_NAME, COU_SEM, COU_YEAR, COU_START_DATE, COU_END_DATE, COURSE_ID")] COURSE cOURSE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOURSE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cOURSE);
        }


        // GET: /Edit/5
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

                    COURSE cOURSE = db.COURSEs.Find(id);
                    if (cOURSE == null)
                    {
                        return HttpNotFound();
                    }
                    return View(cOURSE);
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

        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            COURSE cOURSE = db.COURSEs.Find(id);
            db.COURSEs.Remove(cOURSE);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

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