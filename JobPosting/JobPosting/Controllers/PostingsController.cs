using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobPosting.DAL;
using JobPosting.Models;
using JobPosting.ViewModels;

namespace JobPosting.Controllers
{
    [Authorize]
    public class PostingsController : Controller
    {
        private JBEntities db = new JBEntities();

        // GET: Postings
        public ActionResult Index()
        {
            var postings = db.Postings.Include(p => p.Position);
            ViewBag.JobRequirements = db.JobRequirements.OrderBy(a => a.QualificationID);
            ViewBag.JobLocations = db.JobLocations.OrderBy(a => a.LocationID);
            return View(postings.ToList());
        }

        // GET: Postings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Posting posting = db.Postings.Find(id);
            if (posting == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobRequirements = db.JobRequirements.Where(j => j.PostingID == id);
            ViewBag.JobLocations = db.JobLocations.Where(jl => jl.PostingID == id);
            return View(posting);
        }

        // GET: Postings/Create
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult Create()
        {
            Posting posting = new Posting();
            posting.Days = new List<Day>();

            PopulateDropdownList();
            PopulateQualification();
            ViewBag.Locations = db.Locations;
            PopulateAssignedDay(posting);
            return View();
        }

        

        // POST: Postings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult Create([Bind(Include = "ID,pstNumPosition,pstFTE,pstSalary,pstCompensationType,pstJobDescription,pstOpenDate,pstEndDate,PositionID,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Posting posting, string[] selectedQualification, string[] selectedDay, string[] selectedLocation)
        {
            try
            {
                if (selectedQualification != null)
                {

                    posting.Days = new List<Day>();


                    foreach (var r in selectedQualification)
                    {
                        var qualificateToAdd = db.Qualification.Find(int.Parse(r));
                        JobRequirement jobRequirement = new JobRequirement
                        {
                            Posting = posting,
                            Qualification = qualificateToAdd,
                            PostingID = posting.ID,
                            QualificationID = qualificateToAdd.ID
                        };
                        db.JobRequirements.Add(jobRequirement);

                    }
                    foreach (var l in selectedLocation)
                    {
                        var locationToAdd = db.Locations.Find(int.Parse(l));
                        JobLocation jobLocation = new JobLocation
                        {
                            Posting = posting,
                            Location = locationToAdd,
                            PostingID = posting.ID,
                            LocationID = locationToAdd.ID
                        };
                        db.JobLocations.Add(jobLocation);

                    }
                    foreach (var d in selectedDay)
                    {
                        var dayToAdd = db.Days.Find(int.Parse(d));
                        posting.Days.Add(dayToAdd);
                    }

                }
                if (ModelState.IsValid)
                {
                    db.Postings.Add(posting);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attemps. Try Again!");
            }
            catch (DataException dex)
            {
                if (dex.InnerException.InnerException.Message.Contains("IX_Unique_Code"))
                {
                    ModelState.AddModelError("PositionCode", "Unable to save changes. The Position Code is already existed.");
                }
                else if (dex.InnerException.InnerException.Message.Contains("IX_Unique_Desc"))
                {
                    ModelState.AddModelError("PositionDescription", "Unable to save changes. The Position can not have the same name.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try Again!");
                }
            }

            PopulateDropdownList(posting);
            return View(posting);
        }

        // GET: Postings/Edit/5
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posting posting = db.Postings.Find(id);
            if (posting == null)
            {
                return HttpNotFound();
            }
            PopulateDropdownList(posting);
            PopulateQualification();
            ViewBag.Locations = db.Locations;
            PopulateAssignedDay(posting);

            int realID = id.Value;
            ViewBag.JobRequirements = db.JobRequirements.Where(j => j.PostingID == realID).ToList();
            ViewBag.JobLocations = db.JobLocations.Where(jl => jl.PostingID == realID).ToList();
            return View(posting);
        }

        // POST: Postings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult EditPost(int? id, Byte[] rowVersion, string[] selectedQualification, string[] selectedDay, string[] selectedLocation)
        {
            int realID;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else {
                realID = id.Value;
            }
            var postingToUpdate = db.Postings
                                     .Include(p => p.JobRequirements)
                                     .Where(i => i.ID == realID)
                                     .SingleOrDefault();
            if (User.IsInRole("Hiring Team"))
            {
                if (postingToUpdate.CreatedBy != User.Identity.Name)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
                }
            }
            if (TryUpdateModel(postingToUpdate, "",
                new string[] { "pstNumPosition","pstFTE", "pstSalary", "pstCompensationType", "pstJobDescription", "pstOpenDate", "pstEndDate", "PositionID" }))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        UpdatePositionQualification(selectedQualification, realID, postingToUpdate);
                        UpdatePositionDay(selectedDay, postingToUpdate);
                        UpdateLocation(selectedLocation, realID, postingToUpdate);
                        db.Entry(postingToUpdate).OriginalValues["RowVersion"] = rowVersion;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Posting)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The Posting was deleted.");
                    }
                    else
                    {
                        var databaseValues = (Posting)databaseEntry.ToObject();
                        if (databaseValues.pstNumPosition != clientValues.pstNumPosition)
                            ModelState.AddModelError("pstNumPosition", "Current Value: " + databaseValues.pstNumPosition);
                        if (databaseValues.pstJobDescription != clientValues.pstJobDescription)
                            ModelState.AddModelError("pstJobDescription", "Current Value: " + databaseValues.pstJobDescription);
                        if (databaseValues.pstOpenDate != clientValues.pstOpenDate)
                            ModelState.AddModelError("pstOpenDate", "Current Value: " + String.Format("{0:yyyy-MM-dd HH:mm}", databaseValues.pstOpenDate));
                        if (databaseValues.pstEndDate != clientValues.pstEndDate)
                            ModelState.AddModelError("pstEndDate", "Current Value: " + String.Format("{0:yyyy-MM-dd HH:mm}", databaseValues.pstEndDate));
                        if (databaseValues.PositionID != clientValues.PositionID)
                            ModelState.AddModelError("PositionID", "Current Value: " + db.Positions.Find(databaseValues.PositionID).PositionDescription);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another User. Your changes were not saved.");
                        postingToUpdate.RowVersion = databaseValues.RowVersion;
                    }

                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try Again!");
                }
            }
            PopulateDropdownList(postingToUpdate);
            PopulateQualification();
            ViewBag.Locations = db.Locations;
            PopulateAssignedDay(postingToUpdate);
            ViewBag.JobRequirements = db.JobRequirements.Where(j => j.PostingID == realID);
            ViewBag.JobLocations = db.JobLocations.Where(l => l.PostingID == realID);
            return View(postingToUpdate);
        }

        private void UpdateLocation(string[] selectedLocation, int id, Posting postingToUpdate)
        {
            if (selectedLocation == null)
            {
                postingToUpdate.JobLocations = new List<JobLocation>();
                return;
            }
            var selectedLocationHS = new HashSet<string>(selectedLocation);
            var postingLocations = new HashSet<int>(db.JobLocations.Where(l => l.PostingID == id).Select(l => l.LocationID));

            foreach (var l in db.Locations)
            {
                var LocationToUpdate = db.Locations.Find(l.ID);
                JobLocation jobLocation = new JobLocation
                {
                    Posting = postingToUpdate,
                    Location = LocationToUpdate,
                    PostingID = id,
                    LocationID = l.ID
                };

                if (selectedLocationHS.Contains(l.ID.ToString()))
                {
                    if (!postingLocations.Contains(l.ID))
                    {
                        db.JobLocations.Add(jobLocation);
                    }
                }
                else
                {
                    if (postingLocations.Contains(l.ID))
                    {
                        var selectedItem = (from a in db.JobLocations
                                            where a.PostingID == id && a.LocationID == l.ID
                                            select a).Single();
                        db.JobLocations.Remove(selectedItem);
                    }
                }
            }

        }

        private void UpdatePositionQualification(string[] selectedQualification, int id, Posting postingToUpdate)
        {
            if (selectedQualification == null)
            {
                postingToUpdate.JobRequirements = new List<JobRequirement>();
                return;
            }

            var selectQualificationsHS = new HashSet<string>(selectedQualification);
            var PostingQualifications = new HashSet<int>(db.JobRequirements.Where(j => j.PostingID == id).Select(j => j.QualificationID));

            foreach (var q in db.Qualification)
            {
                var QualificationToUpdate = db.Qualification.Find(q.ID);
                JobRequirement jobRequirement = new JobRequirement
                {
                    Posting = postingToUpdate,
                    Qualification = QualificationToUpdate,
                    PostingID = id,
                    QualificationID = q.ID
                };

                if (selectQualificationsHS.Contains(q.ID.ToString()))
                {
                    if (!PostingQualifications.Contains(q.ID))
                    {
                        db.JobRequirements.Add(jobRequirement);
                    }
                }
                else
                {
                    if (PostingQualifications.Contains(q.ID))
                    {
                        var selectedItem = (from a in db.JobRequirements
                                            where a.PostingID == id && a.QualificationID == q.ID
                                            select a).Single();
                        db.JobRequirements.Remove(selectedItem);

                    }
                }
            }
        }

        private void UpdatePositionDay(string[] selectedDay, Posting postingToUpdate)
        {
            if (selectedDay == null)
            {
                postingToUpdate.Days = new List<Day>();
                return;
            }

            var selectedDaysHS = new HashSet<string>(selectedDay);
            var positionDays = new HashSet<int>(postingToUpdate.Days.Select(p => p.ID));

            foreach (var day in db.Days)
            {
                if (selectedDaysHS.Contains(day.ID.ToString()))
                {
                    if (!positionDays.Contains(day.ID))
                    {
                        postingToUpdate.Days.Add(day);
                    }
                }
                else
                {
                    if (positionDays.Contains(day.ID))
                    {
                        postingToUpdate.Days.Remove(day);
                    }
                }
            }
        }

        // GET: Postings/Delete/5
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posting posting = db.Postings.Find(id);
            if (User.IsInRole("Hiring Team"))
            {
                if (posting.CreatedBy != User.Identity.Name)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
                }
            }
            if (posting == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobRequirements = db.JobRequirements.Where(j => j.PostingID == id).OrderBy(a => a.QualificationID);
            ViewBag.JobLocations = db.JobLocations.Where(jl => jl.PostingID == id);
            return View(posting);
        }

        // POST: Postings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult DeleteConfirmed(int id)
        {
            Posting posting = db.Postings.Find(id);
            try
            {
                db.Postings.Remove(posting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try Again!");
            }
            ViewBag.JobRequirements = db.JobRequirements.Where(j => j.PostingID == id).OrderBy(a => a.QualificationID);
            ViewBag.JobLocations = db.JobLocations.Where(jl => jl.PostingID == id);

            return View(posting);
        }

        private void PopulateDropdownList(Posting posting = null)
        {
            ViewBag.PositionID = new SelectList(db.Positions.OrderBy(p => p.PositionDescription), "ID", "PositionDescription", posting?.PositionID);
        }

        private void PopulateQualification()
        {
            ViewBag.Qualifications = db.Qualification.OrderBy(q => q.QlfDescription);
        }

        private void PopulateAssignedDay(Posting posting)
        {
            var allDays = db.Days;
            var pDays = new HashSet<int>(posting.Days.OrderBy(d => d.dayOrder).Select(d => d.ID));
            var viewModel = new List<DayVM>();
            foreach (var con in allDays)
            {
                viewModel.Add(new DayVM
                {
                    DayID = con.ID,
                    dayName = con.dayName,
                    Assigned = pDays.Contains(con.ID)
                });
            }
            ViewBag.Day = viewModel;
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
