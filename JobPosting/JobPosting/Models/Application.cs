using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    //Only let Applicant delete their own Application
    public class Application : Auditable
    {
        private DateTime applyDate = DateTime.Now;
        public Application()
        {
            this.BinaryFiles = new HashSet<BinaryFile>();
            this.ApplicationsQualifications = new HashSet<ApplicationQualification>();
        }


        public int ID { get; set; }


        //No need Display
        //Number base on the drag and drop position (1 -> infinity), the lowest number is at the highest priority
        public int Priority { get; set; }

        [Display(Name = "Posting")]
        [Required(ErrorMessage = "You have to specify Posting")]
        public int PostingID { get; set; }

        [Display(Name = "Applicant")]
        [Required(ErrorMessage = "You have to specify Applicant.")]
        public int ApplicantID { get; set; }

        //[Display(Name = "Apply Date")]
        //public DateTime ApplyDate
        //{
        //    get { return applyDate; }
        //    set
        //    {
        //        value = applyDate;
        //    }
        //}


        public virtual Posting Posting { get; set; }

        public virtual Applicant Applicant { get; set; }

        //Qualifications that an Applicant have
        public virtual ICollection<ApplicationQualification> ApplicationsQualifications { get; set; }

        public virtual ICollection<BinaryFile> BinaryFiles { get; set; }
    }
}