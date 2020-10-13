using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspMobileQuizOverWifi.Models;

namespace AspMobileQuizOverWifi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Account()
        {
            return View();
        }

        public IActionResult Quizzes()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }
        // LoginPost
        [HttpPost]
        public IActionResult LoginAttempt(string username, string password)
        {

            // Create our asp database object to send through login attempt
            RegisterMobileQuizOverWifi.DatabaseCreator databaseCreator = new RegisterMobileQuizOverWifi.DatabaseCreator();
            // attempt to login with passed through data.
            bool loginStatus = databaseCreator.loginDB(username, password);
            if (loginStatus)
            {
                ViewData["Username"] = username;
                ViewData["Password"] = password;
                return View("Index");
            }
            // if not logged in, return to the login view with viewdata of an error
            ViewData["Error"] = "There was an error";
            return View("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
