using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainingApplication.Models;
using PagedList;

namespace TrainingApplication.Controllers
{
    [Authorize(Roles = "TrainingStaff")]
    public class CourseCatesController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        // GET: CourseCates
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CourseCateNameSortParm = String.IsNullOrEmpty(sortOrder) ? "CourseCateName_desc" : "";
            ViewBag.CourseCateDesSortParm = String.IsNullOrEmpty(sortOrder) ? "CourseCateDes_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var courseCate = from c in db.CourseCates
                             select c;
            if(!String.IsNullOrEmpty(searchString))
            {
                courseCate = courseCate.Where(c => c.CourseCateName.Contains(searchString)
                                                || c.CourseCateDes.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "CourseCateName_desc":
                    courseCate = courseCate.OrderByDescending(c => c.CourseCateName);
                    break;
                case "CourseCateDes_desc":
                    courseCate = courseCate.OrderByDescending(c => c.CourseCateDes);
                    break;
                default:
                    courseCate = courseCate.OrderBy(c => c.CourseCateName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(courseCate.ToPagedList(pageNumber, pageSize));
        }

        // GET: CourseCates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCate courseCate = db.CourseCates.Find(id);
            if (courseCate == null)
            {
                return HttpNotFound();
            }
            return View(courseCate);
        }

        // GET: CourseCates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseCates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseCateID,CourseCateName,CourseCateDes")] CourseCate courseCate)
        {
            if (ModelState.IsValid)
            {
                db.CourseCates.Add(courseCate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(courseCate);
        }

        // GET: CourseCates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCate courseCate = db.CourseCates.Find(id);
            if (courseCate == null)
            {
                return HttpNotFound();
            }
            return View(courseCate);
        }

        // POST: CourseCates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseCateID,CourseCateName,CourseCateDes")] CourseCate courseCate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseCate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(courseCate);
        }

        // GET: CourseCates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCate courseCate = db.CourseCates.Find(id);
            if (courseCate == null)
            {
                return HttpNotFound();
            }
            return View(courseCate);
        }

        // POST: CourseCates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseCate courseCate = db.CourseCates.Find(id);
            db.CourseCates.Remove(courseCate);
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
