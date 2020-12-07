using Site.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdminMobileQuizOverWifi;

namespace Site.Controllers
{
    public class HomeController : Controller
    {
        // connection to DB Entity Framework
        private MobileQuizWifiDBEntities db = new MobileQuizWifiDBEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Permissions()
        {
            return View();
        }

        public ActionResult ModifyQuizzes()
        {
            if (isInstructor())
            {
                return View();
            }
            ViewData["Title"] = "You must be logged in as an instructor to view the Modify Quizzes page";
            return View("LogIn");
        }

        public ActionResult ModifyCourses()
        {
            if (isInstructor())
            {
                return View();
            }
            ViewData["Title"] = "You must be logged in as an instructor to view the Modify Courses page";
            return View("LogIn");
        }

        public ActionResult ModifyGrades()
        {
            if (isInstructor())
            {
                return View();
            }
            ViewData["Title"] = "You must be logged in as an instructor to view the Modify Quizzes page";
            return View("LogIn");
        }

        public ActionResult Quizzes()
        {
            // Check if the user is logged in
            if (loggedIn())
            {
                DatabaseCreator databaseCreator = new DatabaseCreator();

                // if the user is logged into the instructor, have the quizzes page get the data from getQuizzesInstructorDB() which returns all quizzes
                if (isInstructor())
                {
                    return View("Quizzes", databaseCreator.getQuizzesInstructorDB());
                }
                // Else return the quizzes from getQuizzesDB based on the logged in user's id.
                else
                {
                    return View("Quizzes", databaseCreator.getQuizzesDB(getLoggedInUserID()));
                }
            }

            // If not logged in send them to the login page with a message that they need to log in in order to view the quizzes page
            // ViewData["Title"] = "You must log in first in order to view the quizzes page";
            ViewBag.Error = "You must log in first in order to view the Quizzes page.";
            return View("LogIn");
        }

        public ActionResult Account()
        {
            // Check if the user is logged in
            if (loggedIn())
            {
                ViewBag.Message = "Your contact page.";
                return View("Account", isInstructor());

            }

            // If not logged in send them to the login page with a message that they need to log in in order to view the account page
            ViewBag.Error = "You must log in first in order to view the Account page";
            return View("LogIn");
        }

        public ActionResult LogIn()
        {
            ViewData["Title"] = "Log In";
            return View();
        }

        // Method to logout the current session's user
        public ActionResult LogOut()
        {
            // Clear the sessionid stored on the database
            DatabaseCreator databaseCreator = new DatabaseCreator();
            databaseCreator.removeSesssionDB(HttpContext.Session.SessionID);
            // remove sessionId and all cookies from logged in user
            Session.Clear();
            Session.Abandon();
            // return them to the home page.
            return View("LoggedOut");
        }

        public ActionResult GoToQuiz(int quizID)
        {
            // When loading a quiz we need to pass through the question data including question descriptions and possible answers
            DatabaseCreator databaseCreator = new DatabaseCreator();

            // Make our list to hold our questiondata objects we will create
            List<QuestionData> questionDataList = new List<QuestionData>();
            // Grab the string 2d arrays of the questions and answers for the given quizID
            List<string[]> questionList = databaseCreator.getQuestionDataDB(quizID);
            // for getting answer data we need to know all the questionIDs for that quiz, so we make a list of them to pass through.
            List<int> questionIDList = databaseCreator.getQuestionIDsDB(quizID);
            List<string[]> answerData = databaseCreator.getAnswerDataDB(questionIDList);
            // after we have the data we need we can create our questionData object for each item 
            // loop through our questionData 2d array
            foreach(string[] q in questionList)
            {
                // make our temporary ans_ID list to be able to add it to our question data object
                List<int> tempAns_IDList = new List<int>();

                // make our temporary description list to be able to add it to our question data object
                List<string> tempDescriptionList = new List<string>();
                //"SELECT ANS_ID, QUE_ID, DESCRIPTION FROM ANSWER WHERE "; QUE_ID, QUE_QUESTION, TYPE_ID, QUESTION_ANSWER
                // Loop through our list of answer data and place them into our temporary lists if they should be attached to the current question.(ans_id/fk match)
                foreach (string[] a in answerData)
                {
                    if (a[1] == q[0])
                    {
                        tempAns_IDList.Add(System.Convert.ToInt32(a[0]));
                        tempDescriptionList.Add(a[2]);
                    }
                }

                // Create our questionData object and add it to the QuestionDataList if it isn't null
                    questionDataList.Add(new QuestionData
                    {
                        que_ID = q[0],
                        que_question = q[1],
                        type_ID = q[2],
                        ans_IDList = tempAns_IDList,
                        descriptionList = tempDescriptionList
                    });
            }

            ViewBag.QuizName = databaseCreator.getQuizNameDB(quizID);
            ViewBag.Quiz_ID = quizID;
            // pass our new question data object to the quiz view.
            return View("Quiz", questionDataList);
        }

        // Using the cookie for UserName we retrieve that userID
        public int getLoggedInUserID()
        {
            DatabaseCreator databaseCreator = new DatabaseCreator();
            return databaseCreator.getUserIDDB(HttpContext.Session["UserName"].ToString());
        }

        // return true or false depending on if the user is logged in. We check this by seeing if the cookie for session id is empty or null.
        // TODO make this confirm against the server if that session id was not timed out yet.
        public bool loggedIn()
        {
            if (HttpContext.Session["UserSessionID"] != null && HttpContext.Session["UserSessionID"].ToString() != "")
            {
                return true;
            }
            else
                return false;
        }

        // Check if the current user is an instructor.
        public bool isInstructor()
        {
            DatabaseCreator databaseCreator = new DatabaseCreator();
            return databaseCreator.isInstructorDB(getLoggedInUserID());
        }


        // LOGIN 
        [HttpPost]
        public ActionResult LogIn(string username, string password)
        {
            // Get the browser's generated sesssion id
            string session = HttpContext.Session.SessionID;
            // Create our asp database object to send through login attempt
            DatabaseCreator databaseCreator = new DatabaseCreator();
            // attempt to login with passed through data.
            bool loginStatus = databaseCreator.loginDB(username, password, session);

            // If login attempt was successful, display message that they are logged in. Send them back to the home page
            if (loginStatus)
            {
                // Set the session id to an accessible userid as well as a cookie for the name
                HttpContext.Session["UserSessionID"] = HttpContext.Session.SessionID;
                HttpContext.Session["UserName"] = username;
                HttpContext.Session["User_ID"] = databaseCreator.getUserIDDB(username);
                HttpContext.Session["User_isInstructor"] = databaseCreator.isInstructorDB(databaseCreator.getUserIDDB(username));

                // Send them back to the home page logged in
                return View("Index");
            }
            // if not logged in, return to the login view with viewdata of an error
            // TODO Parse the type of error based on the sql return value.
            ViewBag.Error = "There was an error";
            return View("LogIn");
        }

        [HttpPost]
        public ActionResult SubmitQuiz(int qui_ID)
        {
            DatabaseCreator databaseCreator = new DatabaseCreator();
            List<string> responseList = new List<string>();
            List<int> questionList = databaseCreator.getQuestionIDsDB(qui_ID);


            foreach (int i in questionList)
            {
                responseList.Add(Request.Form[i.ToString()]);
            }

            int grade = databaseCreator.gradeQuizDB(qui_ID, responseList, getLoggedInUserID(), databaseCreator.getCourseQuizIDDB(qui_ID, getLoggedInUserID()));
            //ViewBag.QuizName = databaseCreator.getQuizNameDB(quiz_ID);
            // TODO calculate grade
            // TODO update grade table
            ViewBag.Grade = grade;
            return View("Quiz", grade);
        }

        // Change the password of the current user. 
        // TODO Overloaded method to allower userid entry as well to be used by instructor on the config page.
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            DatabaseCreator databaseCreator = new DatabaseCreator();

            // Make sure they entered a password in current password and that is their current password in the db for the given user id
            if (currentPassword != "" && !databaseCreator.CheckPasswordDB(getLoggedInUserID(), currentPassword))
            {
                ViewBag.PasswordChange = "Your current password was incorrect.";
                return View("Account");
            }
            // If the password is empty we tell them it can't be blank
            else if (currentPassword == "")
            {
                ViewBag.PasswordChange = "Current password cannot be blank";
                return View("Account");
            }
            // If their password was correct then we start to verify the new password
            // TODO verify the length is fine and that no special characters exist.
            else
            {
                // Check that neither are blank
                if (newPassword != "" && confirmNewPassword != "")
                {
                    // check that they match
                    if (newPassword == confirmNewPassword)
                    {
                        // make sure the new password is not the same as the old password
                        if (newPassword != currentPassword)
                        {
                            // Call our UpdatePasswordDB method with the username of the current user and a newPassword string
                            databaseCreator.UpdatePasswordDB(HttpContext.Session["UserName"].ToString(), newPassword);
                            ViewBag.PasswordChange = "Your password was successfully changed.";
                        }
                        else
                        {
                            ViewBag.PasswordChange = "Your new password cannot be the same as your current password.";
                            return View("Account");
                        }
                    }
                    else
                    {
                        ViewBag.PasswordChange = "Your new password and confirmation did not match.";
                        return View("Account");
                    }
                }
                else
                {
                    ViewBag.PasswordChange = "Your new password or confirmation cannot be blank.";
                    return View("Account");
                }
            }
            // return to account page after, The ViewBag.PasswordChange will hold the message depending on whether it was succesful or not.
            return View("Account");
        }
    }
}