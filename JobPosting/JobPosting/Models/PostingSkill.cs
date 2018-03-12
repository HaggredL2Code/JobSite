using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class PostingSkill
    {
        [Key, ForeignKey("Posting")]
        [Column(Order = 1)]
        public int PostingID { get; set; }

        [Key, ForeignKey("Skill")]
        [Column(Order = 2)]
        public int SkillID { get; set; }

        public virtual Skill Skill { get; set; }

        public virtual Posting Posting { get; set; }
    }
}