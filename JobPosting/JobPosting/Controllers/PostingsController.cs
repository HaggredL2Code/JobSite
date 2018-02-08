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
    public class PostingsController : Controller
    {
        private JBEntities db = new JBEntities();

        // GET: Postings
        public ActionResult Index()
        {
            var postings = db.Postings.Include(p => p.Position);
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
            return View(posting);
        }

        // GET: Postings/Create
        public ActionResult Create()
        {
            PopulateDropdownList();
            return View();
        }

        

        // POST: Postings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,pstNumPosition,pstJobDescription,pstOpenDate,pstEndDate,PositionID,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Posting posting)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Postings.Add(posting);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try Again!");
            }

            PopulateDropdownList(posting);
            return View(posting);
        }

        // GET: Postings/Edit/5
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
            return View(posting);
        }

        // POST: Postings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, Byte[] rowVersion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var postingToUpdate = db.Postings.Find(id);
            if (TryUpdateModel(postingToUpdate, "",
                new string[] { "pstNumPosition", "pstJobDescription", "pstOpenDate", "pstEndDate", "PositionID" }))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
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
            return View(postingToUpdate);
        }

        // GET: Postings/Delete/5
        public ActionResult Delete(int? id)
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
            return View(posting);
        }

        // POST: Postings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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

            return View(posting);
        }

        private void PopulateDropdownList(Posting posting = null)
        {
            ViewBag.PositionID = new SelectList(db.Positions.OrderBy(p => p.PositionDescription), "ID", "PositionDescription", posting?.PositionID);
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
