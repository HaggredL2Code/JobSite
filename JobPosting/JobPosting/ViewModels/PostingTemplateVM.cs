using JobPosting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPosting.ViewModels
{
    public class PostingTemplateVM
    {
        public IList<PostingTemplate> postingTemplate { get; set; }
    }
}