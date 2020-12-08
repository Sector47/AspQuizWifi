using AdminMobileQuizOverWifi;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Linq;

namespace Site.Controllers
{
    public class ModifyUsersController : Controller
    {
        // DB Controller
        private DBEntities db = new DBEntities();

        // not instructor msg
        string notInstructor = "You must be logged in as an instructor to view this page.";

        // not logged in msg
        string notLoggedIn = "You are not logged in and do not have the correct permissions to view this page.";

        // GET: ModifyUsers
        public ActionResult Index()
        {
            if (loggedIn())
            {
                if (isInstructor())
                {
                    var users = db.USERS.OrderBy(u => u.L_NAME);
                    return View(users.ToList());
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

        public ActionResult Create()
        {
            if(loggedIn())
            {
                if (isInstructor())
                {
                    Users model = new Users();

                    ViewBag.Permission = true;
                    return View(model);
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
        public ActionResult Create([Bind(Include = "FirstName, LastName, IsInstructor, Username, Password")] Users user)
        {

           // CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLower());
            if (ModelState.IsValid)
            {
                string first = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(user.FirstName.ToLower());
                string last = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(user.LastName.ToLower());
                string username = user.Username.ToLower();

                USER userItem = new USER();
                userItem.F_NAME = first;
                userItem.L_NAME = last;
                userItem.USERNAME = username;
                userItem.PASSWORD = user.Password;
                userItem.IS_INSTRUCTOR = user.IsInstructor;

                try
                {
                    db.USERS.Add(userItem);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Msg = "Unable to add. Double check data and retry.";
                }
            }

            return View(user);
        }

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
                    USER user = db.USERS.Find(id);
                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    Users userModel = new Users();
                    userModel.UserID = user.USER_ID;
                    userModel.FirstName = user.F_NAME;
                    userModel.LastName = user.L_NAME;
                    userModel.Username = user.USERNAME;
                    userModel.IsInstructor = user.IS_INSTRUCTOR;
                    userModel.Password = user.PASSWORD;
                    return View(userModel);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID, FirstName, LastName, IsInstructor, UserName, Password")] Users user)
        {


            if (ModelState.IsValid)
            {

                string first = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(user.FirstName.ToLower());
                string last = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(user.LastName.ToLower());
                string username = user.Username.ToLower();

                USER userItem = new USER();
                userItem.USER_ID = user.UserID;
                userItem.F_NAME = first;
                userItem.L_NAME = last;
                userItem.USERNAME = username;
                userItem.PASSWORD = user.Password;
                userItem.IS_INSTRUCTOR = user.IsInstructor;

                try
                {
                    db.Entry(userItem).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Msg = "Unable to update. The data may be invalid or already exists.";
                }              
            }
            return View(user);
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
                    USER user = db.USERS.Find(id);

                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    return View(user);
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