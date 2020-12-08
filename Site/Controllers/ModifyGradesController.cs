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

        public ActionResult Grade(string graid, string uid, string cqid, string ggid, string nid, string onGradePage)
        {
            int userID = int.Parse(uid);
            int courseQuiID = int.Parse(cqid);
            var rESPONSes = (db.RESPONSEs.Where(r => (r.COURSE_QUI_ID == courseQuiID && (r.USER_ID == userID))));

            ViewData["GradeID"] = graid;
            ViewData["UserID"] = uid;
            ViewData["CourseQuiID"] = cqid;
            ViewData["Grade"] = ggid;
            ViewData["NeedsGrading"] = nid;

            return View(rESPONSes);
        }

        public ActionResult Edit(string graid, string uid, string cqid, string ggid, string nid, string onGradePage)
        {
            int gradeID = int.Parse(graid);
            int userID = int.Parse(uid);
            int cqID = int.Parse(cqid);
            int graGrade = int.Parse(ggid);
            bool needsGrading = bool.Parse(nid);
            bool onGradePageFlag = false;
            if (onGradePage != null)
            {
                onGradePageFlag = bool.Parse(onGradePage);
            }

            var query =
                from gra in db.GRADEs
                where gra.GRA_ID == gradeID
                select gra;

            foreach (GRADE gra in query)
            {
                if (graGrade != -1)
                {
                    gra.GRA_GRADE = graGrade;
                }

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