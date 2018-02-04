using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    //this table will not connect to any other tables
    //the data in this table will be deleted when the Interviewer click "Selected All Carted Applicant"
    //the data will be transfer to Interview Table
    public class InterviewCart : IValidatableObject
    {
        public int ID { get; set; }

        //take ID from Application (auto)
        public int ApplicationID { get; set; }

        public DateTime InterviewDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (InterviewDate < DateTime.Now)
            {
                yield return new ValidationResult("Interview Date cannot be in the past.", new[] { "interviewDate" });
            }
        }
    }
}