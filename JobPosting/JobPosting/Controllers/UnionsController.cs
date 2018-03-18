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
    [Authorize(Roles = "Admin, Manager, Hiring Team")]
    public class UnionsController : Controller
    {
        private JBEntities db = new JBEntities();

        // GET: Unions
        public ActionResult Index(string SearchString)
        {

            var unions = db.Unions.Include(a => a.Positions);

            if (!String.IsNullOrEmpty(SearchString))
            {
                unions = unions.Where(u => u.UnionName.ToUpper().Contains(SearchString.ToUpper()));
            }

            return View(unions.ToList());
        }

        // GET: Unions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Union union = db.Unions.Find(id);
            if (union == null)
            {
                return HttpNotFound();
            }
            return View(union);
        }

        // GET: Unions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Unions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UnionName")] Union union)
        {
            if (ModelState.IsValid)
            {
                db.Unions.Add(union);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(union);
        }

        // GET: Unions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Union union = db.Unions.Find(id);
            if (union == null)
            {
                return HttpNotFound();
            }
            return View(union);
        }

        // POST: Unions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UnionName")] Union union)
        {
            if (ModelState.IsValid)
            {
                db.Entry(union).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(union);
        }

        // GET: Unions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Union union = db.Unions.Find(id);
            if (union == null)
            {
                return HttpNotFound();
            }
            return View(union);
        }

        // POST: Unions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Union union = db.Unions.Find(id);
            db.Unions.Remove(union);
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
