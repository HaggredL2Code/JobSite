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
    public class ApplicationCartsController : Controller
    {
        private JBEntities db = new JBEntities();

        // GET: ApplicationCarts
        public ActionResult Index()
        {
            return View(db.ApplicationCarts.ToList());
        }

        // GET: ApplicationCarts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationCart applicationCart = db.ApplicationCarts.Find(id);
            if (applicationCart == null)
            {
                return HttpNotFound();
            }
            return View(applicationCart);
        }

        // GET: ApplicationCarts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Priority,ApplicantID,PostingID")] ApplicationCart applicationCart)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationCarts.Add(applicationCart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationCart);
        }

        // GET: ApplicationCarts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationCart applicationCart = db.ApplicationCarts.Find(id);
            if (applicationCart == null)
            {
                return HttpNotFound();
            }
            return View(applicationCart);
        }

        // POST: ApplicationCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Priority,ApplicantID,PostingID")] ApplicationCart applicationCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationCart);
        }

        // GET: ApplicationCarts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationCart applicationCart = db.ApplicationCarts.Find(id);
            if (applicationCart == null)
            {
                return HttpNotFound();
            }
            return View(applicationCart);
        }

        // POST: ApplicationCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationCart applicationCart = db.ApplicationCarts.Find(id);
            db.ApplicationCarts.Remove(applicationCart);
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
