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
        [Column(Order = 1)]
        public int ApplicationID { get; set; }

        [Key, ForeignKey("Qualification")]
        [Column(Order = 2)]
        public int QualificationID { get; set; }

        public virtual Application Application { get; set; }

        public virtual Qualification Qualification { get; set; }
    }
}