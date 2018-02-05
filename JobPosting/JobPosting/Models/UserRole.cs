using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class UserRole
    {
        public UserRole()
        {
            this.Applicants = new HashSet<Applicant>();
        }

        public int ID { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Role is required.")]
        [Index("IX_Unique_Role", IsUnique = true)]
        [StringLength(100, ErrorMessage = "Role cannot be more than 100 characters.")]
        public string RoleTitle { get; set; }

        public virtual ICollection<Applicant> Applicants { get; set; }
    }
}