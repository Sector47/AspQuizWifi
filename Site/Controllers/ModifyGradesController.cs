using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Site.Models;

namespace Site.Controllers
{
    public class ModifyGradesController : Controller
    {
        // connection to DB Entity Framework
        private DBEntities db = new DBEntities();

        // GET: ModifyGrades
        public ActionResult Index()
        {
            var gRADEs = db.GRADEs.Include(u => u.USER);
            return View(gRADEs.ToList());
        }

        // Populates model with row data from the response table.
        // Filtered to only show a specific quiz.
        public ActionResult Grade(string graid, string uid, string cqid, string ggid, string nid, string onGradePage)
        {
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
    }
}