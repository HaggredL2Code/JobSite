using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class Province
    {
        public Province() {
            this.Cities = new HashSet<City>();
        }

        public int ID { get; set; }


        [Display(Name = "Province")]
        [Required(ErrorMessage = "Province is required.")]
        [StringLength(100, ErrorMessage = "Province cannot be more than 100 characters.")]
        [Index("IX_Unique_Province", IsUnique = true)]
        public string province { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}