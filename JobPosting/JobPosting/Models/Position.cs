using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{

    public class Position
    {
        public Position()
        {
            this.JobLocations = new HashSet<JobLocation>();
            this.JobRequirements = new HashSet<JobRequirement>();
            this.Postings = new HashSet<Posting>();
        }

        public int ID { get; set; }

        [Display(Name = "Job Code")]
        [Required(ErrorMessage = "Job Code is required.")]
        [Index("IX_Unique_Code", IsUnique = true, Order = 1)]
        [StringLength(maximumLength: 5, MinimumLength = 5, ErrorMessage = "Job Code requires 5 characters.")]
        public string PositionCode { get; set; }

        [Display(Name = "Job Description")]
        [Required(ErrorMessage = "Job Description is required.")]
        [Index("IX_Unique_Desc", Order = 2)]
        [StringLength(50, ErrorMessage = "Job Description can not be longer than 50 characters.")]
        public string PositionDescription { get; set; }


        //In View create checkbox for days in a week
        [Display(Name = "Day of Work")]
        [Required(ErrorMessage = "Day of Work is required.")]
        public string PositionDayofWork { get; set; }

        [Display(Name = "FTE")]
        [Required(ErrorMessage = "FTE is required.")]
        public decimal PositionFTE { get; set; }

        [Display(Name = "Salary")]
        [Required(ErrorMessage = "Salary is required.")]
        [DataType(DataType.Currency)]
        public decimal PositionSalary { get; set; }

        //[Display(Name = "Compensation")]
       // [Required(ErrorMessage = "Compensation is required.")]
       // public bool PositionCompensation { get; set; }

        //int 1-3 correspond to Hourly, Monthly, Yearly
        //In the View will need to manually create DropDown List
        [Display(Name = "Job Compensation Type")]
        [Required(ErrorMessage = "Job Compensation Type is required.")]
        [Range(1,3, ErrorMessage = "Invalid Job Compensation Type.")]
        public Int16 PositionCompensationType { get; set; }


        [Required(ErrorMessage = "You have to specify the Job Group.")]
        [Display(Name = "Job Group")]
        public int JobGroupID { get; set; }

        [Required(ErrorMessage = "You have to specify the Union.")]
        [Display(Name = "Union")]
        public int UnionID { get; set; }

        public virtual JobGroup JobGroup { get; set; }

        public virtual Union Union { get; set; }

        public virtual ICollection<JobLocation> JobLocations { get; set; }

        public virtual ICollection<JobRequirement> JobRequirements { get; set; }

        public virtual ICollection<Posting> Postings { get; set; }
    }
}