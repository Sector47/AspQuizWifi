
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
                // user is logged in
                if (isInstructor())
                {
                    // user is instructor, return view with data
                    var users = db.USERS.OrderBy(u => u.L_NAME);
                    return View(users.ToList());
                }
                else
                {
                    // user is student, send permisison msg
                    ViewBag.NotPermitted = notInstructor;
                    return View();
                }
            }
            else
            {
                // user not logged in, send permission msg
                ViewBag.NotPermitted = notLoggedIn;
                return View();
            }
        }

        public ActionResult Create()
        {
            if(loggedIn())
            {
                // user logged in
                if (isInstructor())
                {
                    // user is instructor, send view with a user model to help create the form
                    Users model = new Users();
                    ViewBag.Permission = true;
                    return View(model);
                }
                else
                {
                    // user is student, indicate no permisisons, return view
                    ViewBag.Permission = false;
                    ViewBag.PermissionMsg = notInstructor;
                    return View();
                }
            }
            else
            {
                // user not logged in, indicate permissions, return view
                ViewBag.Permission = false;
                ViewBag.PermissionMsg = notLoggedIn;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName, LastName, IsInstructor, Username, Password")] Users user)
        {
            if (ModelState.IsValid)
            {
                // information that came is valid

                // change first/last name to proper casing
                string first = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(user.FirstName.ToLower());
                string last = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(user.LastName.ToLower());

                // change username to all lower
                string username = user.Username.ToLower();

                // create a user object
                USER userItem = new USER();
                userItem.F_NAME = first;
                userItem.L_NAME = last;
                userItem.USERNAME = username;
                userItem.PASSWORD = user.Password;
                userItem.IS_INSTRUCTOR = user.IsInstructor;

                try
                {
                    // attempt to add user to db
                    db.USERS.Add(userItem);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    // failed, create db failure msg 
                    ViewBag.Msg = "Unable to add. Double check data and retry.";
                }
            }
            
            // return view with the user
            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (loggedIn())
            {
                // user is logged in
                if (isInstructor())
                {
                    // user is instructor
                    if (id == null)
                    {
                        // id is null, cant load an edit page without it
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    // find the user with this id
                    USER user = db.USERS.Find(id);
                    if (user == null)
                    {
                        // if the user is null, error pg
                        return HttpNotFound();
                    }

                    // user exists, create a users model of the USER db item
                    Users userModel = new Users();
                    userModel.UserID = user.USER_ID;
                    userModel.FirstName = user.F_NAME;
                    userModel.LastName = user.L_NAME;
                    userModel.Username = user.USERNAME;
                    userModel.IsInstructor = user.IS_INSTRUCTOR;
                    userModel.Password = user.PASSWORD;

                    // return the view with a user model, to fill the form
                    return View(userModel);
                }
                else
                {
                    // user is student, msg saying not permitted
                    ViewBag.PermissionMsg = notInstructor;
                    return View();
                }
            }
            else
            {
                // user is not logged in, msg saying not permitted
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
                // data the came in seems valid
                // apply proper casing
                string first = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(user.FirstName.ToLower());
                string last = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(user.LastName.ToLower());
                string username = user.Username.ToLower();

                // create a DB user object from the users model
                USER userItem = new USER();
                userItem.USER_ID = user.UserID;
                userItem.F_NAME = first;
                userItem.L_NAME = last;
                userItem.USERNAME = username;
                userItem.PASSWORD = user.Password;
                userItem.IS_INSTRUCTOR = user.IsInstructor;

                try
                {
                    // attempt to update its info
                    db.Entry(userItem).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    // if fails, create msg about failure to update
                    ViewBag.Msg = "Unable to update. The data may be invalid or already exists.";
                }              
            }

            // return user trying to be edited
            return View(user);
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
                        // no id, cannot load page
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    // try to find the user at id that came in
                    USER user = db.USERS.Find(id);

                    if (user == null)
                    {
                        // user doesnt exist in db, error
                        return HttpNotFound();
                    }

                    // return the view of user to be deleted
                    return View(user);
                }
                else
                {
                    // user is student, not permitted msg
                    ViewBag.PermissionMsg = notInstructor;
                    return View();
                }
            }
            else
            {
                // user is not logged in, not permitted msg
                ViewBag.PermissionMsg = notLoggedIn;
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // delete user at that id
            USER user = db.USERS.Find(id);
            db.USERS.Remove(user);
            db.SaveChanges();

            // return to list of users
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
    
        // checks if user is logged in
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