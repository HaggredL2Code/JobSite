using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class ApplicationQualification
    {
        [Key, ForeignKey("Application")]
        public int ApplicationID { get; set; }

        [Key, ForeignKey("Qualification")]
        public int QualificationID { get; set; }

        public virtual Application Application { get; set; }

        public virtual Qualification Qualification { get; set; }
    }
}