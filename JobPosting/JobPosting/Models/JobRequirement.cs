using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class JobRequirement
    {
        [Key, ForeignKey("Position")]
        public int PositionID { get; set; }

        [Key, ForeignKey("Qualification")]
        public int QualificationID { get; set; }

        public virtual Qualification Qualification { get; set; }

        public virtual Position Position { get; set; }
    }
}