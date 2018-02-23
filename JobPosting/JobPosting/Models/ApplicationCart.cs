using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    //This table will not connect to any other tables
    //The data which have the same ApplicantID will be deleted when the Applicant click "Apply all"
    //The data will be transfer to the Application table
    public class ApplicationCart
    {
        public ApplicationCart()
        {
            this.BinaryFileTemps = new HashSet<BinaryFileTemp>();
        }

        public int ID { get; set; }

        //Priority range from 1 -> infinity, the lowest value the highest priority
        public int Priority { get; set; }

        //take ID from applicant (auto)
        public int ApplicantID { get; set; }

        //take ID from posting (auto)
        public int PostingID { get; set; }

        public virtual ICollection<BinaryFileTemp> BinaryFileTemps { get; set; }
    }
}