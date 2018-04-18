using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class Picked
    {
        private int JobTypePrevPicked1 = 0;
        private int JobTypePrevPicked2 = 0;
        private int JobTypeJustPicked = 0;

        [Key, ForeignKey("Applicant")]
        public int PickedID { get; set; }

        public int jobTypePrevPicked1 {
            get {
                return JobTypePrevPicked1;
            }
            set {
                JobTypePrevPicked1 = value;
            }
        }

        public int jobTypePrevPicked2 {
            get {
                return JobTypePrevPicked2;
            }
            set {
                JobTypePrevPicked2 = value;
            }
        }

        public int jobTypeJustPicked {
            get {
                return JobTypeJustPicked;
            }
            set {
                JobTypeJustPicked = value;
            }
        }

        public bool firstTimeAccess { get; set; }

        public virtual Applicant Applicant { get; set; }
    }
}