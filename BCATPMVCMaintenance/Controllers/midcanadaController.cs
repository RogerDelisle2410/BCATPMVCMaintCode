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
    public class MidCanadaController : Controller
    {
        private readonly BCATPEntities db = new BCATPEntities();
        private readonly List<int> allTheSheets = new List<int>();
        // GET: midcanada
        public ActionResult Index(string searchString)
        {
            var midcanada = from s in db.midcanada
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                midcanada = midcanada.Where(s => s.name.Contains(searchString));
            }
            return View(midcanada.ToList());
        }

        // GET: midcanada/Details/5
        public ActionResult Details(int? id, string value)
        {
            midcanada midcanada = db.midcanada.Find(id);
            if (midcanada == null)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong, invalid model");
            }

            foreach (var item in db.midcanada)
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
                midcanada = db.midcanada.Find(ViewBag.allKeys[index + 1]);
            }
            if (index + 1 > ViewBag.allKeys.Count - 1 && value == "next")
            {
                midcanada = db.midcanada.Find(ViewBag.first);
            }
            if (index - 1 >= -1 && value == "prev" && index != 0)
            {
                midcanada = db.midcanada.Find(ViewBag.allKeys[index - 1]);
            }
            if (index == 0 && value == "prev")
            {
                midcanada = db.midcanada.Find(ViewBag.last);
            }

            return View(midcanada);
        }

        // GET: midcanada/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: midcanada/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] midcanada midcanada)
        {
            if (ModelState.IsValid)
            {
                db.midcanada.Add(midcanada);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(midcanada);
        }

        // GET: midcanada/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            midcanada midcanada = db.midcanada.Find(id);
            if (midcanada == null)
            {
                return HttpNotFound();
            }
            return View(midcanada);
        }

        // POST: midcanada/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] midcanada midcanada)
        {
            if (ModelState.IsValid)
            {
                db.Entry(midcanada).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(midcanada);
        }

        // GET: midcanada/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            midcanada midcanada = db.midcanada.Find(id);
            if (midcanada == null)
            {
                return HttpNotFound();
            }
            return View(midcanada);
        }

        // POST: midcanada/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            midcanada midcanada = db.midcanada.Find(id);
            db.midcanada.Remove(midcanada);
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
