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
            this.Locations = new HashSet<Location>();
        }

        public int ID { get; set; }

        [Display(Name ="City")]
        [Required(ErrorMessage = "City is required")]
        [StringLength(100)]
        [Index("IX_Unique_City", IsUnique = true)]
        public string city { get; set; }

        public int provinceID { get; set; }

        [Display(Name = "Province")]
        [Required(ErrorMessage = "Province is required.")]
        public virtual Province Province { get; set; }

        public ICollection<Applicant> Applicants { get; set; }
        public ICollection<Location> Locations { get; set; }
    }
}