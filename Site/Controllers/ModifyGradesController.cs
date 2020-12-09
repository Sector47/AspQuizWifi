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
    public class ModifyGradesController : Controller
    {
        // connection to DB Entity Framework
        private DBEntities db = new DBEntities();
         // not instructor msg
        string notInstructor = "You must be logged in as an instructor to view this page.";
        // not logged in msg
        string notLoggedIn = "You are not logged in and do not have the correct permissions to view this page.";

        // GET: ModifyGrades
        public ActionResult Index()
        {
             if (loggedIn())
            {
                // user is logged in
                if (isInstructor())
                {
                    // user is instructor, return view with data
                    var gRADEs = db.GRADEs.Include(u => u.USER);
                    return View(gRADEs.ToList());
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

        // Populates model with row data from the response table.
        // Filtered to only show a specific quiz.
        public ActionResult Grade(string graid, string uid, string cqid, string ggid, string nid, string onGradePage)
        {
            if (loggedIn())
            {
                // user is logged in
                if (isInstructor())
                {
                   if(graid == null || uid == null ||cqid == null || nid == null)
                    {
                        // data that came in was null ,not okay
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    // Holds selected grade information.
                    int userID = int.Parse(uid);
                    int courseQuiID = int.Parse(cqid);

                    // Holds responses specific to the selected quiz.
                    var rESPONSes = (db.RESPONSEs.Where(r => (r.COURSE_QUI_ID == courseQuiID && (r.USER_ID == userID))));

                    // Holds data to be passed to the Grade table. 
                    ViewData["GradeID"] = graid;
                    ViewData["UserID"] = uid;
                    ViewData["CourseQuiID"] = cqid;
                    ViewData["Grade"] = ggid;
                    ViewData["NeedsGrading"] = nid;

                    // Returns view with a quiz's responses in an IEnumerable model.
                    return View(rESPONSes);
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

        // Action to Update the Grade table with changes to quiz grade.
        public ActionResult Edit(string graid, string uid, string cqid, string ggid, string nid, string onGradePage)
        {
            // Holds data to pass to the Grade table.
            int gradeID = int.Parse(graid);
            int userID = int.Parse(uid);
            int cqID = int.Parse(cqid);
            int graGrade = int.Parse(ggid);
            bool needsGrading = bool.Parse(nid);
            bool onGradePageFlag = false;

            // Check which page to return the view to.
            if (onGradePage != null)
            {
                // Flag used to direct data and view.
                onGradePageFlag = bool.Parse(onGradePage);
            }

            // Query to update the Grade table with point(s).
            var query =
                from gra in db.GRADEs
                where gra.GRA_ID == gradeID
                select gra;

            // Loop to update the grade and the needs grading notification.
            foreach (GRADE gra in query)
            {
                if (graGrade != -1)
                {
                    // Set the grade.
                    gra.GRA_GRADE = graGrade;
                }

                // Sets the notification status.
                gra.GRA_NEEDSGRADING = needsGrading;
            }

            // Submit the changes to the database.
            try
            {
                db.SaveChanges();
                if (onGradePageFlag == true)
                {
                    return RedirectToAction("Grade", new { graid = gradeID.ToString(), uid = userID.ToString(), cqid = cqID.ToString(), ggid = graGrade.ToString(), nid = needsGrading.ToString(), onGradePage = "true" });
                }
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Provide for exceptions.
                return RedirectToAction("Index");
            }
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