using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobPosting.DAL;
using JobPosting.Models;
using System.Data.Entity.Infrastructure;

using NLog;

namespace JobPosting.Controllers
{
    public class PositionsController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private JBEntities db = new JBEntities();

        // GET: Positions
        public ActionResult Index(string sortDirection, string sortField, string actionButton, int? UnionID, string SearchString, int? JobGroupID)
        {

            PopulateDropdownList();

            var positions = db.Positions.Include(p => p.JobGroup).Include(p => p.Union);
            if (UnionID.HasValue)
            {
                positions = positions.Where(u => u.UnionID == UnionID);

            }

            if (JobGroupID.HasValue)
            {
                positions = positions.Where(u => u.JobGroupID == JobGroupID);

            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                positions = positions.Where(p => p.PositionCode.ToUpper().Contains(SearchString.ToUpper()));
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

            if (sortField == "Job Type")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    positions = positions.OrderBy(p => p.JobGroup.GroupTitle);
                }
                else
                {
                    positions = positions.OrderByDescending(p => p.JobGroup.GroupTitle);
                }
            }
            else if (sortField == "Unions")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    positions = positions.OrderBy(p => p.Union.UnionName);
                }
                else
                {
                    positions = positions.OrderByDescending(p => p.Union.UnionName);
                }
            }
            else if (sortField == "Job Code")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    positions = positions.OrderBy(p => p.PositionCode);
                }
                else
                {
                    positions = positions.OrderByDescending(p => p.PositionCode);
                }
            }



            ViewBag.sortField = sortField;
            ViewBag.sortDirection = sortDirection;


            return View(positions.ToList());
        }


        // GET: Positions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                logger.Info("Details/ Bad HTTP Request with ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                logger.Info("Details/ Unable to find Position with ID {0}", id);
                return HttpNotFound();
            }
            return View(position);
        }

        // GET: Positions/Create
        public ActionResult Create()
        {
            PopulateDropdownList();
            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PositionCode,PositionDescription,JobGroupID,UnionID,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Position position)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Positions.Add(position);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                logger.Error("Create/ Retry Limit Exceeded. Unable To Save Changes");
                ModelState.AddModelError("", "Unable to save changes after multiple attemps. Try Again!");
            }
            catch (DataException dex)
            {
                logger.Error("Create/ Data Exception Error {0}", dex.ToString());
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
                logger.Info("Edit/ HTTP Bad Request With ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                logger.Info("Edit/ Unable to find Position with ID {0}", id);
                return HttpNotFound();
            }

            PopulateDropdownList();
            return View(position);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, Byte[] rowVersion)
        {
            var positionToUpdate = db.Positions.Find(id);
                                    
            if (TryUpdateModel(positionToUpdate, "",
                new string[] { "PositionCode", "PositionDescription", "JobGroupID", "UnionID" }))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(positionToUpdate).OriginalValues["RowVersion"] = rowVersion;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (RetryLimitExceededException)
                {
                    logger.Error("EditPost/ Retry Limit Exceeded. Unable To Save Changes");
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try Again!");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    logger.Error("EditPost/ Database Concurrency Exception");
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
                        //if (databaseValues.PositionDayofWork != clientValues.PositionDayofWork)
                        //{
                        //    string day = "";
                        //    foreach (var d in databaseValues.PositionDayofWork)
                        //    {
                        //        day += d.ToString() + ", ";
                        //    }
                        //    ModelState.AddModelError("PositionDatofWork", "Current Value: " + day);
                        //}
                        if (databaseValues.JobGroupID != clientValues.JobGroupID)
                            ModelState.AddModelError("JobGroupID", "Current Value: " + db.JobGroups.Find(databaseValues.JobGroupID).GroupTitle);
                        if (databaseValues.UnionID != clientValues.UnionID)
                            ModelState.AddModelError("UnionID", "Current Value: " + db.Unions.Find(databaseValues.UnionID).UnionName);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another User. Your changes were not saved.");
                        positionToUpdate.RowVersion = databaseValues.RowVersion;

                    }
                }
            }
            PopulateDropdownList(positionToUpdate);
            return View(positionToUpdate);
        }

        // GET: Positions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                logger.Info("Delete/ HTTP Bad Request With ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                logger.Info("Delete/ Unable to find Position ID {0}", id);
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
            try
            {
                db.Positions.Remove(position);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DataException dex)
            {
                if (dex.InnerException.InnerException.Message.Contains("FK_"))
                {
                    logger.Error("DeleteConfirmed/ Attempted to delete Position with Postings attached.");
                    ModelState.AddModelError("", "You cannot delete a Position that have Postings in the System.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try Again!");
                }
            }
            return View(position);

        }

        private void PopulateDropdownList(Position Position = null)
        {
            ViewBag.JobGroupID = new SelectList(db.JobGroups.OrderBy(j => j.GroupTitle), "ID", "GroupTitle", Position?.JobGroupID);
            ViewBag.UnionID = new SelectList(db.Unions.OrderBy(u => u.UnionName), "ID", "UnionName", Position?.UnionID);
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
