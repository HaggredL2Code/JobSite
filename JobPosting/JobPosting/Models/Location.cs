using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class Location
    {
        public Location()
        {
            this.JobLocations = new HashSet<JobLocation>();
        }

        public int ID { get; set; }
        
        public string Address
        {
            get {
                return Street + ", " + City.city + ", " + City.Province.province;
            }
        }


        [Required(ErrorMessage = "Street")]
        [StringLength(50, ErrorMessage = "Street can not be longer than 50 characters.")]
        public string Street { get; set; }

        //[Required(ErrorMessage = "Province")]
        //[StringLength(50, ErrorMessage = "Province can not be longer than 50 characters.")]
        //public string Province { get; set; }

        [Required(ErrorMessage = "You need to specify City.")]
        [Display(Name = "City")]
        public int CityID { get; set; }

        public virtual ICollection<JobLocation> JobLocations { get; set; }

        public virtual City City { get; set; }
    }
}