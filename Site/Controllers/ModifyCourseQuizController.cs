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
    public class ModifyCourseQuizController : Controller
    {

        private DBEntities db = new DBEntities();
        Models.COURSE_QUIZ model = new Models.COURSE_QUIZ();

        // GET: 
        public ActionResult Index()
        {
            model.AllQuizOptions = db.QUIZs.ToList().Select(a => new SelectListItem
            {
                Text = a.QUI_NAME,
                Value = a.QUI_ID.ToString()
            });

            model.AllCourseOptions = db.COURSEs.ToList().Select(b => new SelectListItem
            {
                Text = b.COU_NAME,
                Value = b.COURSE_ID.ToString()
            });

            return View(model);
        }

        // POST: Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Index([Bind(Include = "QUI_ID, COURSE_ID, COURSE_QUI_ID")] COURSE_QUIZ cOURSE_QUIZ)
        public ActionResult Index(COURSE_QUIZ cOURSE_QUIZ)
        {
            if (ModelState.IsValid)
            {
                int uQueID = int.Parse(cOURSE_QUIZ.QuizSelected);
                int uCouID = int.Parse(cOURSE_QUIZ.CourseSelected);
                //int uCouQueID = 1;

                COURSE_QUIZ cq = new COURSE_QUIZ()
                {
                    QUI_ID = uQueID,
                    COURSE_ID = uCouID,
                };
                db.COURSE_QUIZ.Add(cq);
                db.SaveChanges();

                return RedirectToAction("../ModifyQuizzes/Index");
            }

            return View(cOURSE_QUIZ);
        }

    }
}