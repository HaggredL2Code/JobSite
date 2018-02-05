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
    public class InterviewCartsController : Controller
    {
        private JBEntities db = new JBEntities();

        // GET: InterviewCarts
        public ActionResult Index()
        {
            return View(db.InterviewCarts.ToList());
        }

        // GET: InterviewCarts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterviewCart interviewCart = db.InterviewCarts.Find(id);
            if (interviewCart == null)
            {
                return HttpNotFound();
            }
            return View(interviewCart);
        }

        // GET: InterviewCarts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InterviewCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ApplicationID,InterviewDate")] InterviewCart interviewCart)
        {
            if (ModelState.IsValid)
            {
                db.InterviewCarts.Add(interviewCart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(interviewCart);
        }

        // GET: InterviewCarts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterviewCart interviewCart = db.InterviewCarts.Find(id);
            if (interviewCart == null)
            {
                return HttpNotFound();
            }
            return View(interviewCart);
        }

        // POST: InterviewCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ApplicationID,InterviewDate")] InterviewCart interviewCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interviewCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(interviewCart);
        }

        // GET: InterviewCarts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterviewCart interviewCart = db.InterviewCarts.Find(id);
            if (interviewCart == null)
            {
                return HttpNotFound();
            }
            return View(interviewCart);
        }

        // POST: InterviewCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InterviewCart interviewCart = db.InterviewCarts.Find(id);
            db.InterviewCarts.Remove(interviewCart);
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
