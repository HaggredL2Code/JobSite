using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class Posting : Auditable, IValidatableObject
    {
        DateTime openDate = DateTime.Now;
        bool enabled = true;

        public Posting() {
            this.Applications = new HashSet<Application>();
            this.JobRequirements = new HashSet<JobRequirement>();
            this.PostingSkills = new HashSet<PostingSkill>();
            this.JobLocations = new HashSet<JobLocation>();
            this.Days = new HashSet<Day>();
        }

        public int ID { get; set; }

        //[Display(Name = "Job Title")]
        //public string pstJobTitle
        //{
        //    get {
        //        return Job?.JobDescription;
        //    }
        //}

        [Display(Name = "Number of Position")]
        [Required(ErrorMessage = "Number of Position is required.")]
        [Range(1,9999, ErrorMessage = "Invalid Number of Position.")]
        public int pstNumPosition { get; set; }

        [Display(Name = "FTE")]
        [Required(ErrorMessage = "FTE is required.")]
        public decimal pstFTE { get; set; }

        [Display(Name = "Salary")]
        [Required(ErrorMessage = "Salary is required.")]
        [DataType(DataType.Currency)]
        public decimal pstSalary { get; set; }

        [Display(Name = "Job Compensation Type")]
        [Required(ErrorMessage = "Job Compensation Type is required.")]
        [Range(1, 3, ErrorMessage = "Invalid Job Compensation Type.")]
        public Int16 pstCompensationType { get; set; }

        //Job Description is used to tell the Applicant that
        //what the applicant will do in that particular job.
        [Display(Name = "Job Title")]
        [Required(ErrorMessage = "Job Description is required.")]
        public string pstJobDescription { get; set; }

        [Display(Name = "Open Date")]
        [Required(ErrorMessage = "Open Date is required.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime pstOpenDate {
            get {
                return openDate;
            }
            set {
                openDate = value;
            }
        }

        [Display(Name = "Closing Date")]
        [Required(ErrorMessage = "Closing Date is required.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime pstEndDate { get; set; }

        public bool Enabled {
            get { return enabled; }
            set { enabled = value; }
        }

        [Display(Name = "Job Title")]
        [Required(ErrorMessage = "You have specify the Job Title.")]
        public int PositionID { get; set; }
        
        public virtual Position Position { get; set; }

        public virtual ICollection<Application> Applications { get; set; }

        public virtual ICollection<JobRequirement> JobRequirements { get; set; }

        public virtual ICollection<PostingSkill> PostingSkills { get; set; }

        public virtual ICollection<JobLocation> JobLocations { get; set; }

        public virtual ICollection<Day> Days { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (pstEndDate < DateTime.Now)
            {
                yield return new ValidationResult("The closing date cannot be in the past.", new[] { "pstEndDate" });
            }
            if (pstOpenDate > pstEndDate)
            {
                yield return new ValidationResult("The open date cannot be after the closing date.", new[] { "pstOpenDate" });
            }
        }
    }
}