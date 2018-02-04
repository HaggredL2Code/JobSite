using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class JobRequirement
    {
        public int ID { get; set; }

        public int PositionID { get; set; }

        public int QualificationID { get; set; }

        public virtual Qualification Qualification { get; set; }

        public virtual Position Position { get; set; }
    }
}