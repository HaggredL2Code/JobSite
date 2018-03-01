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

namespace JobPosting.Controllers
{
    [Authorize]
    public class ApplicantsController : Controller
    {
        private JBEntities db = new JBEntities();

        // GET: Applicants
        public ActionResult Index()
        {

            return View();
        }

        // GET: Applicants/Details/5
        public ActionResult Details()
        {
            Applicant applicant = db.Applicants.Where(a => a.apEMail == User.Identity.Name).SingleOrDefault();

            if (applicant == null)
            {
                return RedirectToAction("Create");
            }
            return View(applicant);
        }

        // GET: Applicants/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Applicants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,apFirstName,apMiddleName,apLastName,apPhone,apSubscripted,apEMail,apAddress,apPostalCode, UserRoleID")] Applicant applicant)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Applicants.Add(applicant);
                    db.SaveChanges();
                    return RedirectToAction("Details");
                }
            }
            catch (DataException dex)
            {
                if (dex.InnerException.InnerException.Message.Contains("IX_Unique"))
                {
                    ModelState.AddModelError("apEmail", "Email is already existed.");
                }
                else
                {
                    ModelState.AddModelError("", "Unables to save changes. Try Again!");
                }
            }

 
            return View(applicant);
        }

        // GET: Applicants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }

            return View(applicant);
        }

        // POST: Applicants/Edit/5
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
            Applicant applicantToUpdate = db.Applicants.Find(id);
            if (TryUpdateModel(applicantToUpdate, "",
                    new string[] { "apFirstName", "apMiddleName", "apLastName", "apPhone", "apSubscripted", "apEMail", "apAddress", "apCity", "apPostalCode", "cityID", "UserRoleID" }))
            {
                try
                {
                    db.Entry(applicantToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Applicant)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The Applicant was deleted.");
                    }
                    else
                    {
                        var databaseValues = (Applicant)databaseEntry.ToObject();
                        if (databaseValues.apFirstName != clientValues.apFirstName)
                            ModelState.AddModelError("apFirstName", "Current Value: " + databaseValues.apFirstName);
                        if (databaseValues.apMiddleName != clientValues.apMiddleName)
                            ModelState.AddModelError("apMiddleName", "Current Value: " + databaseValues.apMiddleName);
                        if (databaseValues.apLastName != clientValues.apLastName)
                            ModelState.AddModelError("apLastName", "Current Value: " + databaseValues.apLastName);
                        if (databaseValues.apPhone != clientValues.apPhone)
                            ModelState.AddModelError("apPhone", "Current Value: " + String.Format("{0:(###)-###-####}", databaseValues.apPhone));
                        if (databaseValues.apSubscripted != clientValues.apSubscripted)
                            ModelState.AddModelError("apSubscripted", "Current Value: " + databaseValues.apSubscripted);
                        if (databaseValues.apEMail != clientValues.apEMail)
                            ModelState.AddModelError("apEmail", "Current Value: " + databaseValues.apEMail);
                        if (databaseValues.apAddress != clientValues.apAddress)
                            ModelState.AddModelError("apAddress", "Current Value: " + databaseValues.apAddress);
                        if (databaseValues.apPostalCode != clientValues.apPostalCode)
                            ModelState.AddModelError("apPostalCode", "Current Value: " + databaseValues.apPostalCode);
                        if (databaseValues.UserRoleID != clientValues.UserRoleID)
                            ModelState.AddModelError("UserRoleID", "Current Value: " + db.UserRoles.Find(databaseValues.UserRoleID).RoleTitle);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another User. Your changes were not saved.");
                        applicantToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (DataException dex)
                {
                    if (dex.InnerException.InnerException.Message.Contains("IX_Unique"))
                    {
                        ModelState.AddModelError("apEmail", "Email is already existed.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unables to save changes. Try Again!");
                    }
                }
            }
           
            return View(applicantToUpdate);
        }

        // GET: Applicants/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Applicant applicant = db.Applicants.Find(id);
        //    if (applicant == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(applicant);
        //}

        //// POST: Applicants/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Applicant applicant = db.Applicants.Find(id);
        //    try
        //    {
        //        db.Applicants.Remove(applicant);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    catch (DataException)
        //    {
        //        ModelState.AddModelError("", "Unable To save changes. Try Again!");
        //    }
        //    return View(applicant);
        //}

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
