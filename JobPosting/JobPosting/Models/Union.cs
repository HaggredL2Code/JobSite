using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class Union
    {
        public Union()
        {
            this.Positions = new HashSet<Position>();
        }

        public int ID { get; set; }

        [Display(Name = "Union Name")]
        [Required(ErrorMessage = "Union Name is required.")]
        [Index("IX_Unique_Name", IsUnique = true)]
        [StringLength(100, ErrorMessage = "Union Name can not be longer than 100 characters.")]
        public string UnionName { get; set; }

        public virtual ICollection<Position> Positions { get; set; }
    }
}