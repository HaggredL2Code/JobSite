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
    public class PositionsController : Controller
    {
        private JBEntities db = new JBEntities();

        // GET: Positions
        public ActionResult Index()
        {
            var positions = db.Positions.Include(p => p.JobGroup).Include(p => p.Union);
            return View(positions.ToList());
        }

        // GET: Positions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // GET: Positions/Create
        public ActionResult Create()
        {

            PopulateDropdownList();
            PopulateAssignedQualificationDate();
            return View();
        }

        

        // POST: Positions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PositionCode,PositionDescription,PositionDayofWork,PositionFTE,PositionSalary,PositionCompensation,PositionCompensationType,JobGroupID,UnionID")] Position position, string[] selectedQualification)
        {
            try
            {
                if (selectedQualification != null)
                {
                    position.JobRequirements = new List<JobRequirement>();

                    
                    foreach (var r in selectedQualification)
                    {
                        var qualificateToAdd = db.Qualification.Find(int.Parse(r)).ID;
                        JobRequirement jobRequirement = new JobRequirement
                        {
                            PositionID = position.ID,
                            QualificationID = qualificateToAdd
                        };
                        db.JobRequirements.Add(jobRequirement);

                    }

                }
                if (ModelState.IsValid)
                {
                    db.Positions.Add(position);
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

            PopulateDropdownList(position);
            return View(position);
        }

        // GET: Positions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            PopulateDropdownList(position);
            PopulateAssignedQualificationDate();

            return View(position);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, Byte[] rowVersion ,string[] selectedQualification)
        {
            int id2;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else {
                id2 = id.Value; // convert int? to int
            }
            var positionToUpdate = db.Positions
                                    .Include(p => p.JobRequirements)
                                    .Where(i => i.ID == id)
                                    .SingleOrDefault();
            if (TryUpdateModel(positionToUpdate, "",
                new string[] { "PositionCode", "PositionDescription", "PositionDayofWork", "PositionFTE", "PositionSalary", "PositionCompensation", "PositionCompensationType", "JobGroupID", "UnionID" }))
            {
                try
                {
                    UpdatePositionQualification(selectedQualification, id2);
                    db.Entry(positionToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try Again!");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Position)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The Position was deleted by another User.");
                    }
                    else
                    {
                        var databaseValues = (Position)databaseEntry.ToObject();
                        if (databaseValues.PositionCode != clientValues.PositionCode)
                            ModelState.AddModelError("PositionCode", "Current Value: " + databaseValues.PositionCode);
                        if (databaseValues.PositionDescription != clientValues.PositionDescription)
                            ModelState.AddModelError("PositionDescription", "Current Value: " + databaseValues.PositionDescription);
                        if (databaseValues.PositionDayofWork != clientValues.PositionDayofWork)
                        {
                            string day = "";
                            foreach (var d in databaseValues.PositionDayofWork)
                            {
                                day += d.ToString() + ", ";
                            }
                            ModelState.AddModelError("PositionDatofWork", "Current Value: " + day);
                        }
                        if (databaseValues.PositionFTE != clientValues.PositionFTE)
                        
                    }
                }
            }
            
            PopulateDropdownList(positionToUpdate);
            return View(positionToUpdate);
        }

        private void UpdatePositionQualification(string[] selectedQualification, int id)
        {
            if (selectedQualification == null)
            {
                return;
            }
            var selectQualificationsHS = new HashSet<string>(selectedQualification);
            var PositionQualifications = new HashSet<int>(db.JobRequirements.Where(j => j.PositionID == id).Select(j => j.QualificationID));

            foreach (var q in db.Qualification)
            {
                JobRequirement jobRequirement = new JobRequirement
                {
                    PositionID = id,
                    QualificationID = q.ID
                };

                if (selectQualificationsHS.Contains(q.ID.ToString()))
                {
                   
                    if (!PositionQualifications.Contains(q.ID))
                    { 
                        db.JobRequirements.Add(jobRequirement);
                    }
                }
                else
                {
                    if (PositionQualifications.Contains(q.ID))
                    {
                        db.JobRequirements.Remove(jobRequirement);
                    }
                }
            }
        }

        // GET: Positions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Position position = db.Positions.Find(id);
            db.Positions.Remove(position);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void PopulateDropdownList(Position Position = null)
        {
            ViewBag.JobGroupID = new SelectList(db.JobGroups.OrderBy(j => j.GroupTitle), "ID", "GroupTitle", Position?.JobGroupID);
            ViewBag.UnionID = new SelectList(db.Unions.OrderBy(u => u.UnionName), "ID", "UnionName", Position?.UnionID);
        }

        private void PopulateAssignedQualificationDate()
        {
            var allQualifications = db.Qualification;
            var pQualifications = new HashSet<int>(db.Qualification.Select(q => q.ID));
            var viewModel = new List<QualificationVM>();
            foreach (var con in allQualifications)
            {
                viewModel.Add(new QualificationVM
                {
                    QualificationID = con.ID,
                    QualificationName = con.QlfDescription,
                    Assigned = pQualifications.Contains(con.ID)
                });
            }
            ViewBag.Qualifications = viewModel;
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
