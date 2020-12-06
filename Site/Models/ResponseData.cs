using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models
{
    public class ResponseData
    {
        public int response_ID { get; set; }
        public string comments { get; set; }
        public int course_Qui_ID { get; set; }
        public int que_ID { get; set; }
        public int user_ID { get; set; }
    }
}