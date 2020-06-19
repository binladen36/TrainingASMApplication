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
    public class Trainee_CourseController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        // GET: Trainee_Course
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TraineeNameSortParm = String.IsNullOrEmpty(sortOrder) ? "traineeName_desc" : "";
            ViewBag.CourseNameSortParm = String.IsNullOrEmpty(sortOrder) ? "courseName_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var trainee_Course = db.Trainee_Course.Include(t => t.Course).Include(t => t.Trainee);

            if (!String.IsNullOrEmpty(searchString))
            {
                trainee_Course = trainee_Course.Where(t => t.Course.CourseName.Contains(searchString)
                                          || t.Trainee.TraineeName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "traineeName_desc":
                    trainee_Course = trainee_Course.OrderByDescending(t => t.Trainee.TraineeName);
                    break;
                case "courseName_desc":
                    trainee_Course = trainee_Course.OrderByDescending(t => t.Course.CourseName);
                    break;
                default:
                    trainee_Course = trainee_Course.OrderBy(t => t.Course.CourseName);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(trainee_Course.ToPagedList(pageNumber, pageSize));
        }

        // GET: Trainee_Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainee_Course trainee_Course = db.Trainee_Course.Find(id);
            if (trainee_Course == null)
            {
                return HttpNotFound();
            }
            return View(trainee_Course);
        }

        // GET: Trainee_Course/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.TraineeID = new SelectList(db.Trainees, "TraineeID", "TraineeUserID");
            return View();
        }

        // POST: Trainee_Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TraineeCourseID,TraineeID,CourseID")] Trainee_Course trainee_Course)
        {
            if (ModelState.IsValid)
            {
                db.Trainee_Course.Add(trainee_Course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", trainee_Course.CourseID);
            ViewBag.TraineeID = new SelectList(db.Trainees, "TraineeID", "TraineeUserID", trainee_Course.TraineeID);
            return View(trainee_Course);
        }

        // GET: Trainee_Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainee_Course trainee_Course = db.Trainee_Course.Find(id);
            if (trainee_Course == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", trainee_Course.CourseID);
            ViewBag.TraineeID = new SelectList(db.Trainees, "TraineeID", "TraineeUserID", trainee_Course.TraineeID);
            return View(trainee_Course);
        }

        // POST: Trainee_Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TraineeCourseID,TraineeID,CourseID")] Trainee_Course trainee_Course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainee_Course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", trainee_Course.CourseID);
            ViewBag.TraineeID = new SelectList(db.Trainees, "TraineeID", "TraineeUserID", trainee_Course.TraineeID);
            return View(trainee_Course);
        }

        // GET: Trainee_Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainee_Course trainee_Course = db.Trainee_Course.Find(id);
            if (trainee_Course == null)
            {
                return HttpNotFound();
            }
            return View(trainee_Course);
        }

        // POST: Trainee_Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trainee_Course trainee_Course = db.Trainee_Course.Find(id);
            db.Trainee_Course.Remove(trainee_Course);
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
