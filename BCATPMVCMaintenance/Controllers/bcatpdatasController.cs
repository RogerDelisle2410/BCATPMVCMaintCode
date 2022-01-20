using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BCATPMVCMaintenance.Models;

namespace BCATPMVCMaintenance.Controllers
{
    public class BcatpdatasController : Controller
    {
        private readonly BCATPEntities db = new BCATPEntities();
        private readonly List<int> allTheSheets = new List<int>();
        // GET: bcatpdatas
        public ActionResult Index(string searchString)
        {
            var bcatpdata = from s in db.bcatps
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                bcatpdata = bcatpdata.Where(s => s.name.Contains(searchString));
            }

            return View(bcatpdata.ToList());
        }

        // GET: bcatpdatas/Details/5
        public ActionResult Details(int? id, string value)
        {
            bcatp bcatp = db.bcatps.Find(id);
            if (bcatp == null)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong, invalid model");
            }

            foreach (var item in db.bcatps)
            {
                allTheSheets.Add(item.Id);
            }

            for (int i = 0; i < allTheSheets.Count; i++)
            {
                ViewBag.Current = id;
                if (i == 0)
                {
                    ViewBag.first = allTheSheets[i];
                }
                if (i == allTheSheets.Count - 1)
                {
                    ViewBag.last = allTheSheets[i];
                }
            }

            ViewBag.allKeys = allTheSheets;

            int index = ViewBag.allKeys.IndexOf((int)id);
            if (index + 1 <= ViewBag.allKeys.Count - 1 && value == "next")
            {
                bcatp = db.bcatps.Find(ViewBag.allKeys[index + 1]);
            }
            if (index + 1 > ViewBag.allKeys.Count - 1 && value == "next")
            {
                bcatp = db.bcatps.Find(ViewBag.first);
            }
            if (index - 1 >= -1 && value == "prev" && index != 0)
            {
                bcatp = db.bcatps.Find(ViewBag.allKeys[index - 1]);
            }
            if (index == 0 && value == "prev")
            {
                bcatp = db.bcatps.Find(ViewBag.last);
            }

            return View(bcatp);
        }

        // GET: bcatpdatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: bcatpdatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,latitude,longitude,comment,wiki")] bcatp bcatpdata)
        {
            if (ModelState.IsValid)
            {
                db.bcatps.Add(bcatpdata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bcatpdata);
        }

        // GET: bcatpdatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bcatp bcatpdata = db.bcatps.Find(id);
            if (bcatpdata == null)
            {
                return HttpNotFound();
            }
            return View(bcatpdata);
        }

        // POST: bcatpdatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] bcatp bcatpdata)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bcatpdata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bcatpdata);
        }

        // GET: bcatpdatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bcatp bcatpdata = db.bcatps.Find(id);
            if (bcatpdata == null)
            {
                return HttpNotFound();
            }
            return View(bcatpdata);
        }

        // POST: bcatpdatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bcatp bcatpdata = db.bcatps.Find(id);
            db.bcatps.Remove(bcatpdata);
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
