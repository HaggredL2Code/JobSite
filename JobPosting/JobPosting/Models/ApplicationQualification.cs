using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class ApplicationQualification
    {
        public int ID { get; set; }

        public int ApplicationID { get; set; }

        public int QualificationID { get; set; }

        public virtual Application Application { get; set; }

        public virtual Qualification Qualification { get; set; }
    }
}