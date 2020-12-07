﻿using System;
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
    public class ModifyQuestionsController : Controller
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

            var qUESTIONs = db.QUESTIONs.Include(q => q.QUE_ID);
            if (id == null && ViewBag.QuizID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if(ViewBag.QuizID != null)
            {
                ViewData["QuizID"] = ViewBag.QuizID;
                return View(db.QUESTIONs.ToList().Where(q => q.QUI_ID == ViewBag.QuestionID));
            }

            ViewBag.QuizID = id;
            ViewData["QuizID"] = id;
            return View(db.QUESTIONs.ToList().Where(q => q.QUI_ID == id));
            

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
        public ActionResult Create(int? qui_ID)
        {
            ViewData["QuizID"] = qui_ID;
            return View();
        }
        // POST: Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QUI_ID, QUE_QUESTION, TYPE_ID, QUESTION_ANSWER")] QUESTION qUESTION)
        {
            if (ModelState.IsValid)
            {
                db.QUESTIONs.Add(qUESTION);
                db.SaveChanges();
                return RedirectToAction("../ModifyQuestions/Index", new { id = qUESTION.QUI_ID });
            }

            return View(qUESTION);
        }

        // GET: /Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QUESTION qUESTION = db.QUESTIONs.Find(id);
            if (qUESTION == null)
            {
                return HttpNotFound();
            }
            return View(qUESTION);
        }

        // POST: /Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QUI_ID, QUE_QUESTION, TYPE_ID, QUE_ID, QUESTION_ANSWER")] QUESTION qUESTION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qUESTION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../ModifyQuestions/Index", new {id = qUESTION.QUI_ID });
            }
            return View(qUESTION);
        }


        // GET: /Edit/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            QUESTION qUESTION = db.QUESTIONs.Find(id);
            if (qUESTION == null)
            {
                return HttpNotFound();
            }
            return View(qUESTION);
        }

        // POST: /Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QUESTION qUESTION = db.QUESTIONs.Find(id);
            db.QUESTIONs.Remove(qUESTION);
            db.SaveChanges();
            return RedirectToAction("../ModifyQuestions/Index", new { id = qUESTION.QUI_ID });
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