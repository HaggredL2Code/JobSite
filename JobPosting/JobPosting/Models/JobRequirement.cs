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
        [Key, ForeignKey("Posting")]
        [Column(Order = 1)]
        public int PostingID { get; set; }

        [Key, ForeignKey("Qualification")]
        [Column(Order = 2)]
        public int QualificationID { get; set; }

        public virtual Qualification Qualification { get; set; }

        public virtual Posting Posting { get; set; }
    }
}