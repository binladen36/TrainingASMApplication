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
    public class Course_CourseCateController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        // GET: Course_CourseCate
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CourseNameSortParm = String.IsNullOrEmpty(sortOrder) ? "courseName_desc" : "";
            ViewBag.CourseCateNameSortParm = String.IsNullOrEmpty(sortOrder) ? "CourseCateName_desc" : "";
            
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var course_CourseCate = db.Course_CourseCate.Include(c => c.Course).Include(c => c.CourseCate);

            if (!String.IsNullOrEmpty(searchString))
            {
                course_CourseCate = course_CourseCate.Where(c => c.Course.CourseName.Contains(searchString)
                                          || c.CourseCate.CourseCateName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "courseName_desc":
                    course_CourseCate = course_CourseCate.OrderByDescending(c => c.Course.CourseName);
                    break;
                case "CourseCateName_desc":
                    course_CourseCate = course_CourseCate.OrderByDescending(c => c.CourseCate.CourseCateName);
                    break;
                default:
                    course_CourseCate = course_CourseCate.OrderBy(c => c.Course.CourseName);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(course_CourseCate.ToPagedList(pageNumber, pageSize));
        }

        // GET: Course_CourseCate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course_CourseCate course_CourseCate = db.Course_CourseCate.Find(id);
            if (course_CourseCate == null)
            {
                return HttpNotFound();
            }
            return View(course_CourseCate);
        }

        // GET: Course_CourseCate/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.CourseCateID = new SelectList(db.CourseCates, "CourseCateID", "CourseCateName");
            return View();
        }

        // POST: Course_CourseCate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseCourseCateID,CourseID,CourseCateID")] Course_CourseCate course_CourseCate)
        {
            if (ModelState.IsValid)
            {
                db.Course_CourseCate.Add(course_CourseCate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", course_CourseCate.CourseID);
            ViewBag.CourseCateID = new SelectList(db.CourseCates, "CourseCateID", "CourseCateName", course_CourseCate.CourseCateID);
            return View(course_CourseCate);
        }

        // GET: Course_CourseCate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course_CourseCate course_CourseCate = db.Course_CourseCate.Find(id);
            if (course_CourseCate == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", course_CourseCate.CourseID);
            ViewBag.CourseCateID = new SelectList(db.CourseCates, "CourseCateID", "CourseCateName", course_CourseCate.CourseCateID);
            return View(course_CourseCate);
        }

        // POST: Course_CourseCate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseCourseCateID,CourseID,CourseCateID")] Course_CourseCate course_CourseCate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course_CourseCate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", course_CourseCate.CourseID);
            ViewBag.CourseCateID = new SelectList(db.CourseCates, "CourseCateID", "CourseCateName", course_CourseCate.CourseCateID);
            return View(course_CourseCate);
        }

        // GET: Course_CourseCate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course_CourseCate course_CourseCate = db.Course_CourseCate.Find(id);
            if (course_CourseCate == null)
            {
                return HttpNotFound();
            }
            return View(course_CourseCate);
        }

        // POST: Course_CourseCate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course_CourseCate course_CourseCate = db.Course_CourseCate.Find(id);
            db.Course_CourseCate.Remove(course_CourseCate);
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
