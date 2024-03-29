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
    public class ModifyCourseQuizController : Controller
    {
        // Respresents the connection to the Database Entities used in this project.
        private DBEntities db = new DBEntities();
        Models.COURSE_QUIZ model = new Models.COURSE_QUIZ();
        // not instructor msg
        string notInstructor = "You must be logged in as an instructor to view this page.";
        // not logged in msg
        string notLoggedIn = "You are not logged in and do not have the correct permissions to view this page.";

        // GET: 
        public ActionResult Index()
        {
            if (loggedIn())
            {
                if (isInstructor())
                {
                    model.AllQuizOptions = db.QUIZs.ToList().Select(a => new SelectListItem
                    {
                        Text = a.QUI_NAME,
                        Value = a.QUI_ID.ToString()
                    });

            model.AllCourseOptions = db.COURSEs.ToList().Select(b => new SelectListItem
            {
                Text = b.COU_NAME + "  (Year: " + b.COU_YEAR + ", Sem: " + b.COU_SEM + ")",
                Value = b.COURSE_ID.ToString()
            });

                    return View(model);
                }
                else
                {
                    ViewBag.NotPermitted = notInstructor;
                    return View();
                }
            }
            {
                ViewBag.NotPermitted = notLoggedIn;
                return View();
            }

                  
        }

        // POST: Create a relationship between the a course and quiz.
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(COURSE_QUIZ cOURSE_QUIZ)
        {
            // Check to see the the model is valid and they used made a proper selection to create a relationship.
            if (ModelState.IsValid && cOURSE_QUIZ.QuizSelected != null && cOURSE_QUIZ.CourseSelected != null)
            {
                // Holds user's quiz selections.
                int uQueID = int.Parse(cOURSE_QUIZ.QuizSelected);
                int uCouID = int.Parse(cOURSE_QUIZ.CourseSelected);

                // Empty course_quiz model
                COURSE_QUIZ cq = new COURSE_QUIZ()
                {
                    // Assigns the user values to the table.
                    QUI_ID = uQueID,
                    COURSE_ID = uCouID,
                };

                // Add to table and saves.
                db.COURSE_QUIZ.Add(cq);
                db.SaveChanges();

                // Redirect to the index to allow the user to make another choice.
                return RedirectToAction("../ModifyQuizzes/Index");
            }
            // Model or selections were not valid, redirects user back to index.
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