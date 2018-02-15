using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    //Archive will not have any connection with other tables
    public class Archive
    {
        public int ID { get; set; }

        //Auto Fill in control, therefore no need Required.
        [Display(Name = "Employee Full Name")]
        public string EmployeeName { get; set; }

        //Auto Fill in control, therefore no need Required.
        [Display(Name = "Employee Phone Number")]
        public string EmployeePhone { get; set; }

        //Auto Fill in control, therefore no need Required.
        [Display(Name = "Employee Address")]
        public string EmployeeAddress { get; set; }

        //Auto Fill in control, therefore no need Required.
        [Display(Name = "Employee Position")]
        public string EmployeePosition { get; set; }


    }
}