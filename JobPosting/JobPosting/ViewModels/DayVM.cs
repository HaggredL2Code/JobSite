using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPosting.ViewModels
{
    public class DayVM
    {
        public int DayID { get; set; }
        public string dayName { get; set; }
        public bool Assigned { get; set; }
    }
}