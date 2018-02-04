using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
        internal interface IAuditable
        {
            string CreatedBy { get; set; }
            DateTime? CreatedOn { get; set; }
            string UpdatedBy { get; set; }
            DateTime? UpdatedOn { get; set; }
        }

        public abstract class Auditable : IAuditable
        {
            [ScaffoldColumn(false)]
            [StringLength(256)]
            public string CreatedBy { get; set; }

            [ScaffoldColumn(false)]
            public DateTime? CreatedOn { get; set; }

            [ScaffoldColumn(false)]
            [StringLength(256)]
            public string UpdatedBy { get; set; }

            [ScaffoldColumn(false)]
            public DateTime? UpdatedOn { get; set; }

            [ScaffoldColumn(false)]
            [Timestamp]
            public Byte[] RowVersion { get; set; }//Added for concurrency
        }
    
}