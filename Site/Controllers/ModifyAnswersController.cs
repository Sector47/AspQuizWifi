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
    public class ModifyAnswersController : Controller
    {
        // connection to DB Entity Framework
        private DBEntities db = new DBEntities();

        // GET: ModifyQuestions
        public ActionResult Index(int? id)
        {
            if (!isInstructor())
            {
                ViewData["Title"] = "You must be logged in as an instructor to view the Modify Quizzes page";
                return View("../Home/LogIn");
            }

            var aNSWERs = db.ANSWERs.Include(a => a.ANS_ID);
            if (id == null && ViewBag.QuestionID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if (ViewBag.QuestionID != null)
            {
                ViewData["QuestionID"] = ViewBag.QuestionID;
                return View(db.ANSWERs.ToList().Where(a => a.QUE_ID == ViewBag.QuestionID));
            }

            ViewBag.QuestionID = id;
            ViewData["QuestionID"] = id;
            return View(db.ANSWERs.ToList().Where(a => a.QUE_ID == id));


        }

        // Check if the current user is an instructor.
        public bool isInstructor()
        {

            DatabaseCreator databaseCreator = new DatabaseCreator();
            return databaseCreator.isInstructorDB(getLoggedInUserID());
        }

        public int getLoggedInUserID()
        {
            DatabaseCreator databaseCreator = new DatabaseCreator();
            return databaseCreator.getUserIDDB(HttpContext.Session["UserName"].ToString());
        }

        // GET: ManageQuizzes/Create
        public ActionResult Create(int? que_ID)
        {
            ViewData["QuestionID"] = que_ID;
            return View();
        }
        // POST: Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QUE_ID, DESCRIPTION, CORRECT_ANS")] ANSWER aNSWER)
        {
            if (ModelState.IsValid)
            {
                db.ANSWERs.Add(aNSWER);
                db.SaveChanges();
                return RedirectToAction("../ModifyAnswers/Index", new { id = aNSWER.QUE_ID });
            }

            return View(aNSWER);
        }

        // GET: /Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ANSWER aNSWER = db.ANSWERs.Find(id);
            if (aNSWER == null)
            {
                return HttpNotFound();
            }
            return View(aNSWER);
        }

        // POST: /Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QUE_ID, DESCRIPTION, CORRECT_ANS")] ANSWER aNSWER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aNSWER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../ModifyAnswers/Index", new { id = aNSWER.QUE_ID });
            }
            return View(aNSWER);
        }


        // GET: /Edit/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ANSWER aNSWER = db.ANSWERs.Find(id);
            if (aNSWER == null)
            {
                return HttpNotFound();
            }
            return View(aNSWER);
        }

        // POST: /Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ANSWER aNSWER = db.ANSWERs.Find(id);
            db.ANSWERs.Remove(aNSWER);
            db.SaveChanges();
            return RedirectToAction("../ModifyAnswers/Index", new { id = aNSWER.QUE_ID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}