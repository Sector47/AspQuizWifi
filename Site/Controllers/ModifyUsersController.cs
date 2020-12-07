using AdminMobileQuizOverWifi;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "F_NAME, L_NAME, IS_INSTRUCTOR, USERNAME, PASSWORD")] USER user)
        {
            if (ModelState.IsValid)
            {
                db.USERS.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER user = db.USERS.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USER_ID, F_NAME, L_NAME, IS_INSTRUCTOR, USERNAME, PASSWORD")] USER user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER user = db.USERS.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            USER user = db.USERS.Find(id);
            db.USERS.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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