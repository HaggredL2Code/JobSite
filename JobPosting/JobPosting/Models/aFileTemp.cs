using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    //A temporary File table
    //CASCADE delete on ApplicationCart (Will be deleted when Applicant click Apply All Application )
    //All the File will be transfered to the aFile table 
    public class aFileTemp
    {
        public int ID { get; set; }

        [Display(Name = "File Name")]
        [StringLength(256)]
        public string fileName { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public virtual FileContentTemp FileContentTemp { get; set; }

        public int ApplicationCartID { get; set; }

        public virtual ApplicationCart ApplicationCart { get; set; }

    }
}