using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class Interview : IValidatableObject
    {

        //default value, when transfer data from interviewCart to Interview we dont need to give Accepted value.
        private bool accepted = false;

         
        [Key, ForeignKey("Application")]
        public int InterviewID { get; set; }

        [Display(Name = "Interview Date")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Interview Date is required.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime interviewDate { get; set; }

        public bool Accepted {
            get {
                return accepted;
            }
            set {
                accepted = value;
            }
        }

        public virtual Application Application { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (interviewDate < DateTime.Now)
            {
                yield return new ValidationResult("Interview Date cannot be in the past.", new[] { "interviewDate" });
            }
        }
    }
}