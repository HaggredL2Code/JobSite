﻿using System;
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
    public class ArchivesController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private JBEntities db = new JBEntities();

        // GET: Archives
        public ActionResult Index()
        {
            return View(db.Archives.ToList());
        }

        // GET: Archives/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                logger.Info("Details/ HTTP Bad Request With ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archive archive = db.Archives.Find(id);
            if (archive == null)
            {
                logger.Info("Details/ Failed to find Archive with ID {0}", id);
                return HttpNotFound();
            }
            return View(archive);
        }

        // GET: Archives/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Archives/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeName,EmployeePhone,EmployeeAddress,EmployeePosition")] Archive archive)
        {
            if (ModelState.IsValid)
            {
                db.Archives.Add(archive);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(archive);
        }

        // GET: Archives/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                logger.Info("Edit/ HTTP Bad Request With ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archive archive = db.Archives.Find(id);
            if (archive == null)
            {
                logger.Info("Edit/ Failed to find Archive with ID {0}", id);
                return HttpNotFound();
            }
            return View(archive);
        }

        // POST: Archives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeName,EmployeePhone,EmployeeAddress,EmployeePosition")] Archive archive)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archive).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(archive);
        }

        // GET: Archives/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                logger.Info("Delete/ HTTP Bad Request With ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archive archive = db.Archives.Find(id);
            if (archive == null)
            {
                logger.Info("Delete/ Failed to find Archive with ID {0}", id);
                return HttpNotFound();
            }
            return View(archive);
        }

        // POST: Archives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Archive archive = db.Archives.Find(id);
            db.Archives.Remove(archive);
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
