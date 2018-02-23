using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class BinaryFile
    {
        public int ID { get; set; }

        [Display(Name = "File Name")]
        [StringLength(256)]
        public string FileName { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public virtual FileContent FileContent { get; set; }

        public int ApplicationID { get; set; }

        public virtual Application Application { get; set; }

    }
}