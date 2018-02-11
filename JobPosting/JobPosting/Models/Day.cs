using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class Day
    {


        public int ID { get; set; }

        [Display(Name = "Day Name")]
        [Required(ErrorMessage = "You cannot leave the Day name blank.")]
        [StringLength(50, ErrorMessage = "Too Big!")]
        [Index("IX_Unique_Day", IsUnique = true)]
        public string dayName { get; set; }

        
        public int dayOrder { get; set; }

        public virtual ICollection<Position> Positions { get; set; }
    }
}