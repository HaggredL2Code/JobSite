using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class Skill
    {
        public Skill()
        {
            this.PostingSkills = new HashSet<PostingSkill>();
        }

        public int ID { get; set; }

        [Display(Name = "Skill Description")]
        [Required(ErrorMessage = "Skill Description is required.")]
        [Index("IX_Unique_SkillDesc", IsUnique = true)]
        [StringLength(255, ErrorMessage = "Skill Description can not be longer than 255 characters.")]
        public string SkillDescription { get; set; }

        public virtual ICollection<PostingSkill> PostingSkills { get; set; }
        
        public virtual ICollection<ApplicationSkill> ApplicationSkill { get; set; }
    }
}