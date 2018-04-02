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
    public class JobGroupsController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private JBEntities db = new JBEntities();

        // GET: JobGroups
        public ActionResult Index(string SearchString)
        {
            var jobGroups = db.JobGroups.Include(a => a.Positions);

            if (!String.IsNullOrEmpty(SearchString))
            {
                jobGroups = jobGroups.Where(l => l.GroupTitle.ToUpper().Contains(SearchString.ToUpper()));
            }

            return View(jobGroups.ToList());
        }
        // GET: JobGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                logger.Info("Details/ HTTP Bad Request With ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobGroup jobGroup = db.JobGroups.Find(id);
            if (jobGroup == null)
            {
                logger.Info("Details/ Failed to find Jobgroup with ID {0}", id);
                return HttpNotFound();
            }
            return View(jobGroup);
        }

        // GET: JobGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,GroupTitle")] JobGroup jobGroup)
        {
            if (ModelState.IsValid)
            {
                db.JobGroups.Add(jobGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobGroup);
        }

        // GET: JobGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                logger.Info("Edit/ HTTP Bad Request With ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobGroup jobGroup = db.JobGroups.Find(id);
            if (jobGroup == null)
            {
                logger.Info("Edit/ Failed to find Jobgroup with ID {0}", id);
                return HttpNotFound();
            }
            return View(jobGroup);
        }

        // POST: JobGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,GroupTitle")] JobGroup jobGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobGroup);
        }

        // GET: JobGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                logger.Info("Delete/ HTTP Bad Request With ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobGroup jobGroup = db.JobGroups.Find(id);
            if (jobGroup == null)
            {
                logger.Info("Delete/ Failed to find Jobgroup with ID {0}", id);
                return HttpNotFound();
            }
            return View(jobGroup);
        }

        // POST: JobGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobGroup jobGroup = db.JobGroups.Find(id);
            db.JobGroups.Remove(jobGroup);
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
