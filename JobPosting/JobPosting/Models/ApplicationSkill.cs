using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class ApplicationSkill
    {
        [Key, ForeignKey("Application")]
        [Column(Order = 1)]
        public int ApplicationId { get; set; }

        [Key, ForeignKey("Skill")]
        [Column(Order = 2)]
        public int skillID { get; set; }

        public virtual Skill Skill { get; set; }

        public virtual Application Application { get; set; }
    }
}