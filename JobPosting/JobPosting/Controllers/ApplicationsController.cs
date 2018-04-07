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
    [Authorize]
    public class ApplicationsController : Controller
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        private JBEntities db = new JBEntities();

        // GET: Applications
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult Index(string sortDirection, string sortField, string actionButton, string SearchPrioity, int? PostingID)
        {
            IQueryable<Application> applications = db.Applications.Include(a => a.Applicant).Include(a => a.Posting).Include(a => a.BinaryFiles).Include(a => a.ApplicationsQualifications).Include(a => a.ApplicationSkills);

            if (TempData["NumPositionFlag"] != null)
             {
                ViewBag.NumPositionFlag = TempData["NumPositionFlag"];
                ViewBag.ID = TempData["ID"];
             }
            else
            {
                ViewBag.NumPositionFlag = true;
                
            }
         
            PopulateDropdownList();

            if (PostingID.HasValue)
            {
                applications = applications.Where(a => a.PostingID == PostingID);
            }

            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted
            {
                if (actionButton != "Filter")//Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = String.IsNullOrEmpty(sortDirection) ? "desc" : "";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }

            if (sortField == "Job Code")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    applications = applications.OrderBy(p => p.Posting.Position.PositionCode);
                }
                else
                {
                    applications = applications.OrderByDescending(p => p.Posting.Position.PositionCode);
                }
            }
            else if (sortField == "Applicant Name")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    applications = applications.OrderBy(p => p.Applicant.apFullName);
                }
                else
                {
                    applications = applications.OrderByDescending(p => p.Applicant.apFullName);
                }
            }
            else if (sortField == "Priority")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    applications = applications.OrderBy(p => p.Priority);
                }
                else
                {
                    applications = applications.OrderByDescending(p => p.Priority);
                }
            }



            ViewBag.sortField = sortField;
            ViewBag.sortDirection = sortDirection;


            return View(applications.ToList());

        }

        // GET: Applications/Details/5
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
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

            var pJobRequirements = (from jr in db.JobRequirements
                                    join q in db.Qualification on jr.QualificationID equals q.ID
                                    where jr.PostingID == postingID
                                    select q);
            ViewBag.jobRequirements = new MultiSelectList(pJobRequirements, "ID", "QlfDescription");

        }

        private void PopulatePostingSkills(int postingID)
        {
            var pSkills = (from ps in db.PostingSkills
                           join s in db.Skills on ps.SkillID equals s.ID
                           where ps.PostingID == postingID
                           select s);
            ViewBag.postingSkills = new MultiSelectList(pSkills, "ID", "SkillDescription");
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
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
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
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
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

        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult Accept(int? id, int? numPosition = null)
        {
            var applicant = (from ap in db.Applicants
                             join apl in db.Applications on ap.ID equals apl.ApplicantID
                             where apl.ID == id
                             select ap).SingleOrDefault();
            var job = db.Applications.Find(id).Posting.Position.PositionDescription;
            var application = db.Applications.Find(id);
            var posting = db.Applications.Find(id).Posting;
            Archive archive = new Archive
            {
                EmployeeName = applicant.apFullName,
                EmployeePhone = applicant.apPhone.ToString(),
                EmployeeAddress = applicant.apAddress,
                EmployeePosition = job.ToString()

            };
            if (numPosition != null)
            {
                posting.pstNumPosition = (int)numPosition;
            }
            
            

            if (posting.pstNumPosition != 0)
            {
                TempData["NumPositionFlag"] = null;
                posting.pstNumPosition -= 1;
                db.Entry(posting).State = EntityState.Modified;
                application.Available = false;
                db.Entry(application).State = EntityState.Modified;

            }
            else {
                TempData["NumPositionFlag"] = false;
                TempData["ID"] = id;
                return RedirectToAction("Index");
            } 
            
            db.Archives.Add(archive);
            db.SaveChanges();
            
            return RedirectToAction("Index");
        }
        // GET: Applications/Delete/5
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
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
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public FileContentResult Download(int id)
        {
            var resumeFile = db.BinaryFiles.Include(f => f.FileContent).Where(f => f.ID == id).SingleOrDefault();
            return File(resumeFile.FileContent.Content, resumeFile.FileContent.MimeType, resumeFile.FileName);
        }

        private void PopulateDropdownList(Application Application = null)
        {
            ViewBag.PostingID = new SelectList(db.Postings.OrderBy(j => j.PositionID), "ID", "pstJobDescription", Application?.PostingID);
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
