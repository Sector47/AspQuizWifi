//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GRADE
    {
        public int GRA_ID { get; set; }
        public int USER_ID { get; set; }
        public int COURSE_QUI_ID { get; set; }
        public int GRA_GRADE { get; set; }
    
        public virtual COURSE_QUIZ COURSE_QUIZ { get; set; }
        public virtual USER USER { get; set; }
    }
}