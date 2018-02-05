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

namespace JobPosting.Controllers
{
    public class ArchievesController : Controller
    {
        private JBEntities db = new JBEntities();

        // GET: Archieves
        public ActionResult Index()
        {
            return View(db.Archieves.ToList());
        }

        // GET: Archieves/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archieve archieve = db.Archieves.Find(id);
            if (archieve == null)
            {
                return HttpNotFound();
            }
            return View(archieve);
        }

        // GET: Archieves/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Archieves/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeName,EmployeePhone,EmployeeAddress,EmployeePosition")] Archieve archieve)
        {
            if (ModelState.IsValid)
            {
                db.Archieves.Add(archieve);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(archieve);
        }

        // GET: Archieves/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archieve archieve = db.Archieves.Find(id);
            if (archieve == null)
            {
                return HttpNotFound();
            }
            return View(archieve);
        }

        // POST: Archieves/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeName,EmployeePhone,EmployeeAddress,EmployeePosition")] Archieve archieve)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archieve).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(archieve);
        }

        // GET: Archieves/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archieve archieve = db.Archieves.Find(id);
            if (archieve == null)
            {
                return HttpNotFound();
            }
            return View(archieve);
        }

        // POST: Archieves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Archieve archieve = db.Archieves.Find(id);
            db.Archieves.Remove(archieve);
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
