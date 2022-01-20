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
    public class NaviesController : Controller
    {
        private readonly BCATPEntities db = new BCATPEntities();
        private readonly List<int> allTheSheets = new List<int>();

        // GET: navies
        public ActionResult Index(string searchString)
        {
            var navy = from s in db.navies
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                navy = navy.Where(s => s.name.Contains(searchString));
            }
            return View(navy.ToList());
        }

        //public ActionResult Details(int? id)
        //{
        //    //var maxItem = db.navies.Count();
        //    //if (id == 0)
        //    //{
        //    //    id = maxItem;
        //    //}
        //    //if (id > maxItem)
        //    //{
        //    //    id = 1002;
        //    //}
        //    //if (id == null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}
        //    navy navy = db.navies.Find(id);
        //    if (navy == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(navy);
        //}

        // GET: navies/Details/5
        public ActionResult Details(int? id, string value)
        {
            navy navy = db.navies.Find(id);
            if (navy == null)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong, invalid model");
            }

            foreach (var item in db.navies)
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
                navy = db.navies.Find(ViewBag.allKeys[index + 1]);
            }
            if (index + 1 > ViewBag.allKeys.Count - 1 && value == "next")
            {
                navy = db.navies.Find(ViewBag.first);
            }
            if (index - 1 >= -1 && value == "prev" && index != 0)
            {
                navy = db.navies.Find(ViewBag.allKeys[index - 1]);
            }
            if (index == 0 && value == "prev")
            {
                navy = db.navies.Find(ViewBag.last);
            }

            return View(navy);
        }

        // GET: navies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: navies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] navy navy)
        {
            if (ModelState.IsValid)
            {
                db.navies.Add(navy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(navy);
        }

        // GET: navies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            navy navy = db.navies.Find(id);
            if (navy == null)
            {
                return HttpNotFound();
            }
            return View(navy);
        }

        // POST: navies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] navy navy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(navy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(navy);
        }

        // GET: navies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            navy navy = db.navies.Find(id);
            if (navy == null)
            {
                return HttpNotFound();
            }
            return View(navy);
        }

        // POST: navies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            navy navy = db.navies.Find(id);
            db.navies.Remove(navy);
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
