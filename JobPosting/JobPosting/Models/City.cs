using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class City
    {
        public City()
        {
            this.Applicants = new HashSet<Applicant>();
        }

        public int ID { get; set; }

        [Display(Name ="City")]
        [Required(ErrorMessage = "City is required")]
        [Index("IX_Unique_city", IsUnique = true)]
        public string city { get; set; }

        public ICollection<Applicant> Applicants { get; set; }
    }
}