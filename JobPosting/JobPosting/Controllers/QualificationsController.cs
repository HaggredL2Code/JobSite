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

using NLog;

namespace JobPosting.Controllers
{
    [Authorize(Roles = "Admin, Manager, Hiring Team")]
    public class QualificationsController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private JBEntities db = new JBEntities();

        // GET: Qualifications
        public ActionResult Index(string sortDirection, string sortField, string actionButton, string SearchString)
        {

            var qualifications = db.Qualification.Include(a => a.JobRequirements);

            if (!String.IsNullOrEmpty(SearchString))
            {
                qualifications = qualifications.Where(u => u.QlfDescription.ToUpper().Contains(SearchString.ToUpper()));
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

            if (sortField == "Qualification")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    qualifications = qualifications.OrderBy(q => q.QlfDescription);
                }
                else
                {
                    qualifications = qualifications.OrderByDescending(q => q.QlfDescription);
                }
            }

            ViewBag.sortField = sortField;
            ViewBag.sortDirection = sortDirection;

            return View(qualifications.ToList());
        }

        // GET: Qualifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                logger.Info("Details/ Bad HTTP Request with ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qualification qualification = db.Qualification.Find(id);
            if (qualification == null)
            {
                logger.Info("Details/ Failed to find Qualification with ID {0}", id);
                return HttpNotFound();
            }
            return View(qualification);
        }

        // GET: Qualifications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Qualifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,QlfDescription")] Qualification qualification)
        {
            if (ModelState.IsValid)
            {
                db.Qualification.Add(qualification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(qualification);
        }

        // GET: Qualifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                logger.Info("Edit/ Bad HTTP Request with ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qualification qualification = db.Qualification.Find(id);
            if (qualification == null)
            {
                logger.Info("Edit/ Failed to find Qualification with ID {0}", id);
                return HttpNotFound();
            }
            return View(qualification);
        }

        // POST: Qualifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,QlfDescription")] Qualification qualification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qualification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(qualification);
        }

        // GET: Qualifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                logger.Info("Delete/ Bad HTTP Request with ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qualification qualification = db.Qualification.Find(id);
            if (qualification == null)
            {
                logger.Info("Delete/ Failed to find Qualification with ID {0}", id);
                return HttpNotFound();
            }
            return View(qualification);
        }

        // POST: Qualifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Qualification qualification = db.Qualification.Find(id);
            db.Qualification.Remove(qualification);
            db.SaveChanges();
            return RedirectToAction("Index");
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
