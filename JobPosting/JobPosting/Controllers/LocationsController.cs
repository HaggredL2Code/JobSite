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
    public class LocationsController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private JBEntities db = new JBEntities();

        // GET: Locations
        public ActionResult Index(string sortDirection, string sortField, string actionButton, string SearchString)
        {

            var locations = db.Locations.Include(a => a.JobLocations);

            if (!String.IsNullOrEmpty(SearchString))
            {
                locations = locations.Where(l => l.Address.ToUpper().Contains(SearchString.ToUpper()));
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

            if (sortField == "Address")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                   locations = locations.OrderBy(p => p.Address);
                }
                else
                {
                    locations = locations.OrderByDescending(p => p.Address);
                }
            }


            ViewBag.sortField = sortField;
            ViewBag.sortDirection = sortDirection;

            return View(locations.ToList());
        }


        // GET: Locations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                logger.Info("Details/ Bad HTTP Request with ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                logger.Info("Details/ Unable to find Location with ID {0}", id);
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CityID,Address")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                logger.Info("Edit/ Bad HTTP Request with ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                logger.Info("Edit/ Unable to find Location with ID {0}", id);
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CityID,Address")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                logger.Info("Delete/ Bad HTTP Request with ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                logger.Info("Delete/ Unable to find Location with ID {0}", id);
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
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
