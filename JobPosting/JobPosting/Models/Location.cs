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
        { get; set; }



        public virtual ICollection<JobLocation> JobLocations { get; set; }
    }
}