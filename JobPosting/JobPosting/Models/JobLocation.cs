using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class JobLocation
    {
        
        public int ID { get; set; }

        public int PositionID { get; set; }

        public int LocationID { get; set; }

        public virtual Position Position { get; set; }

        public virtual Location Location { get; set; }
    }
}