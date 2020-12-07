﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models
{
    public class QuestionData
    {
        public string que_ID { get; set; }
        public string que_question { get; set; }

        public string type_ID { get; set; }
        public ANS_TYPE typeID{ get; set; }

        public List<int> ans_IDList { get; set; }
        public List<string> descriptionList { get; set; }

        //public string[,] questionData { get; set; }
        //public string[,] answerData { get; set; }
    }

    public enum ANS_TYPE
    {
        FB,
        MCC,
        MCR,
        SA
    }
}