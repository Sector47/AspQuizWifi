using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Models
{
    public partial class COURSE_QUIZ
    {
        public IEnumerable<SelectListItem> AllQuizOptions { get; internal set; }
        public IEnumerable<SelectListItem> AllCourseOptions { get; internal set; }
        public string QuizSelected { get; set; }
        public string CourseSelected { get; set; }
    }
}
