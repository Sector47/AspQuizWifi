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
    
    public partial class COURSE_QUIZ
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COURSE_QUIZ()
        {
            this.GRADEs = new HashSet<GRADE>();
            this.RESPONSEs = new HashSet<RESPONSE>();
        }
    
        public int QUI_ID { get; set; }
        public int COURSE_ID { get; set; }
        public int COURSE_QUI_ID { get; set; }
    
        public virtual COURSE COURSE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GRADE> GRADEs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RESPONSE> RESPONSEs { get; set; }
        public virtual QUIZ QUIZ { get; set; }
    }
}
