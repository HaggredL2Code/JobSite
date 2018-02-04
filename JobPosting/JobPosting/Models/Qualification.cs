using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class Qualification
    {
        public Qualification()
        {
            this.JobRequirements = new HashSet<JobRequirement>();
            this.ApplicationQualifications = new HashSet<ApplicationQualification>();
        }

        public int ID { get; set; }

        [Display(Name = "Qualification Description")]
        [Required(ErrorMessage = "Qualificate Description is required.")]
        [Index("IX_Unique_QlfDesc", IsUnique = true)]
        [StringLength(255, ErrorMessage = "Qualificate Description can not be longer than 255 characters.")]
        public string QlfDescription { get; set; }

        //Qualification for a Job
        public virtual ICollection<JobRequirement> JobRequirements { get; set; }

        //Qualifications that an Applicant have
        public virtual ICollection<ApplicationQualification> ApplicationQualifications { get; set; }
    }
}