using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobPosting.DAL;
using JobPosting.Models;
using JobPosting.ViewModels;

using NLog;

namespace JobPosting.Controllers
{
    public class ApplicationsController : Controller
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        private JBEntities db = new JBEntities();

        // GET: Applications
        public ActionResult Index()
        {
            IQueryable<Application> applications = db.Applications.Include(a => a.Applicant).Include(a => a.Posting).Include(a => a.BinaryFiles).Include(a => a.ApplicationsQualifications).Include(a => a.ApplicationSkills);
            return View(applications.ToList());
        }

        // GET: Applications/Details/5
        public ActionResult Details(int? id, int? postingID)
        {
            if (id == null || postingID == null)
            {
                if (id == null)
                    logger.Info("Details/ HTTP Bad Request With ID {0}", id);
                if (postingID == null)
                    logger.Info("Details/{0} HTTP Bad Request With Posting ID", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            var posting = (from p in db.Postings
                           where p.ID == postingID
                           select p).SingleOrDefault();
            if (application == null)
            {
                logger.Info("Details/ Application not found with ID {0}", id);
                return HttpNotFound();
            }
            ViewBag.posting = posting;
            PopulateJobRequirements(posting.ID);
            PopulatePostingSkills(posting.ID);
            return View(application);
        }

        // GET: Applications/Create
        public ActionResult Create(int? id)
        {

            if (id == null)
            {
                logger.Info("Create/ HTTP Bad Request With ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var posting = (from p in db.Postings
                           where p.ID == id
                           select p).ToList().SingleOrDefault();
            var applicant = db.Applicants.Where(a => a.apEMail == User.Identity.Name).Select(a => a.ID).SingleOrDefault();

            

            ViewBag.posting = posting;
            ViewBag.applicantID = applicant;
            ViewBag.postingId = id;
            PopulateJobRequirements(posting.ID);
            PopulatePostingSkills(posting.ID);

            return View();
        }

        private void PopulateJobRequirements(int postingID)
        {
            List<string> QualificationName = new List<string>();
            var jobRequirements = db.JobRequirements.Where(jr => jr.PostingID == postingID);
            foreach (var jr in jobRequirements)
            {
                QualificationName.Add(jr.Qualification.QlfDescription);
                
            }
            ViewBag.jobRequirements = jobRequirements; //used to get ID of requirements
            ViewBag.qualificationName = QualificationName; //used to display Desc of qualifications

        }

        private void PopulatePostingSkills(int postingID)
        {
            List<string> SkillName = new List<string>();
            var postingSkills = db.PostingSkills.Where(ps => ps.PostingID == postingID);
            foreach (var ps in postingSkills)
            {
                SkillName.Add(ps.Skill.SkillDescription);
            }
            ViewBag.postingSkills = postingSkills;
            ViewBag.SkillName = SkillName;
        }






        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Priority,Comment,PostingID,ApplicantID")] Application application, int? id, IEnumerable<HttpPostedFileBase> theFiles, string[] selectedQualification, string[] selectedSkill)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bool Valid = true;
            var applicationToCheck = db.Applications.Where(a => a.PostingID == application.PostingID && a.ApplicantID == application.ApplicantID).SingleOrDefault();
            try
            {
                if (selectedQualification != null) // null exception
                {
                    foreach (var q in selectedQualification) // loop through each value of the array
                    {
                        var qualificateToAdd = db.Qualification.Find(int.Parse(q)); // receive record from dataset by the id
                        // create new ApplicationQualification Object to add to the ApplicationQualification table
                        ApplicationQualification applicationQualification = new ApplicationQualification
                        {
                            Application = application,
                            ApplicationID = application.ID,
                            Qualification = qualificateToAdd,
                            QualificationID = qualificateToAdd.ID
                        };
                        db.ApplicationQualification.Add(applicationQualification);
                    }
                }
                if (selectedSkill != null)
                {
                    foreach (var s in selectedSkill)
                    {
                        var skillToAdd = db.Skills.Find(int.Parse(s));
                        ApplicationSkill applicationSkill = new ApplicationSkill
                        {
                            Application = application,
                            ApplicationId = application.ID,
                            Skill = skillToAdd,
                            skillID = skillToAdd.ID
                        };
                        db.ApplicationSkills.Add(applicationSkill);
                    }
                }
                if (ModelState.IsValid)
                {
                    AddDocuments(ref application, out Valid, theFiles);

                    if (Valid)
                    {
                        if (applicationToCheck != null)
                        {
                            db.Applications.Remove(applicationToCheck);
                        }
                       

                        db.Applications.Add(application);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Postings");
                    }
                    ModelState.AddModelError("", "You only be able to submit PDF or MS Word files.");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }

            var posting = (from p in db.Postings
                           where p.ID == id
                           select p).ToList();
            var applicant = db.Applicants.Where(a => a.apEMail == User.Identity.Name).Select(a => a.ID).SingleOrDefault();

            ViewBag.posting = posting;
            ViewBag.applicantID = applicant;
            ViewBag.postingId = id;
            return View(application);
        }

        private void AddDocuments(ref Application application,  out bool Valid,IEnumerable<HttpPostedFileBase> docs)
        {
            Valid = true;
            foreach (var f in docs)
            {
                if (f != null)
                {
                    string mimeType = f.ContentType;
                    if (mimeType == "application/pdf" || mimeType == "application/msword" || mimeType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || mimeType == "application/vnd.openxmlformats-officedocument.wordprocessingml.template")
                    {
                        Valid = true;

                        string fileName = Path.GetFileName(f.FileName);
                        int fileLength = f.ContentLength;
                        if (!(fileName == "" || fileLength == 0))
                        {
                            Stream fileStream = f.InputStream;
                            byte[] fileData = new byte[fileLength];
                            fileStream.Read(fileData, 0, fileLength);

                            BinaryFile newFile = new BinaryFile
                            {
                                FileContent = new FileContent
                                {
                                    Content = fileData,
                                    MimeType = mimeType
                                },
                                FileName = fileName
                            };
                            application.BinaryFiles.Add(newFile);
                        }
                    }
                    else {
                        Valid = false;
                    }
                }
            }
        }

        // GET: Applications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicantID = new SelectList(db.Applicants, "ID", "apFirstName", application.ApplicantID);

            ViewBag.PostingID = new SelectList(db.Postings, "ID", "pstJobDescription", application.PostingID);
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Priority,PostingID,ApplicantID,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicantID = new SelectList(db.Applicants, "ID", "apFirstName", application.ApplicantID);
            ViewBag.PostingID = new SelectList(db.Postings, "ID", "pstJobDescription", application.PostingID);
            return View(application);
        }

        // GET: Applications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public FileContentResult Download(int id)
        {
            var resumeFile = db.BinaryFiles.Include(f => f.FileContent).Where(f => f.ID == id).SingleOrDefault();
            return File(resumeFile.FileContent.Content, resumeFile.FileContent.MimeType, resumeFile.FileName);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
