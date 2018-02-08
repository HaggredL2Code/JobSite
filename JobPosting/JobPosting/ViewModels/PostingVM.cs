using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobPosting.Models;
using JobPosting.DAL;

namespace JobPosting.ViewModels
{
    public class PostingVM
    {
        public IEnumerable<Posting> postings { get; set; }

        public IEnumerable<Qualification> qualifications { get; set; }

        public IEnumerable<Position> positions { get; set; }

        public IEnumerable<Application> application { get; set; }

        public IEnumerable<Location> locations { get; set; }

        public IEnumerable<JobGroup> jobGroup { get; set; }

        public IEnumerable<Union> unions { get; set; }

        public IEnumerable<JobLocation> JobLocation { get; set; }

        public IEnumerable<JobRequirement> jobRequirements { get; set; }
    }
}