using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class JobGroup
    {
        public JobGroup()
        {
            this.Positions = new HashSet<Position>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "Group Title is required.")]
        [Index("IX_Unique_Title", IsUnique = true)]
        [Display(Name = "Job Type")]
        [StringLength(100, ErrorMessage = "Group Title can not be longer than 100 characters.")]
        public string GroupTitle { get; set; }

        public virtual ICollection<Position> Positions { get; set; }
    }
}