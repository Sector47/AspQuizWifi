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
    public class ModifyQuizzesController : Controller
    {
        // connection to DB Entity Framework
        private DBEntities db = new DBEntities();

        // GET: ModifyQuizzes
        public ActionResult Index()
        {
            if (isInstructor())
            {
                var qUIZs = db.QUIZs.Include(q => q.QUI_NAME);
                return View(db.QUIZs.ToList());
            }
            ViewData["Title"] = "You must be logged in as an instructor to view the Modify Quizzes page";
            return View("../Home/LogIn");
           
        }

        // GET: ManageQuizzes/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QUI_NAME, QUI_NOTES, QUI_ID")] QUIZ qUIZ)
        {
            if (ModelState.IsValid)
            {
                db.QUIZs.Add(qUIZ);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(qUIZ);
        }

        // GET: /Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QUIZ qUIZ = db.QUIZs.Find(id);
            if (qUIZ == null)
            {
                return HttpNotFound();
            }
            return View(qUIZ);
        }

        // POST: /Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QUI_NAME, QUI_NOTES, QUI_ID")] QUIZ qUIZ)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qUIZ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(qUIZ);
        }

        // GET: /Edit/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            QUIZ qUIZ = db.QUIZs.Find(id);
            if (qUIZ == null)
            {
                return HttpNotFound();
            }
            return View(qUIZ);
        }

        // POST: /Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QUIZ qUIZ = db.QUIZs.Find(id);
            db.QUIZs.Remove(qUIZ);
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