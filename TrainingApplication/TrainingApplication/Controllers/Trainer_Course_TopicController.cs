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
    public class Trainer_Course_TopicController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        // GET: Trainer_Course_Topic
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TrainerNameSortParm = String.IsNullOrEmpty(sortOrder) ? "trainerName_desc" : "";
            ViewBag.CourseNameSortParm = String.IsNullOrEmpty(sortOrder) ? "courseName_desc" : "";
            ViewBag.TopicNameSortParm = String.IsNullOrEmpty(sortOrder) ? "topicName_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var trainer_Course_Topic = db.Trainer_Course_Topic.Include(t => t.Course).Include(t => t.Topic).Include(t => t.Trainer);

            if (!String.IsNullOrEmpty(searchString))
            {
                trainer_Course_Topic = trainer_Course_Topic.Where(t => t.Course.CourseName.Contains(searchString)
                                                                    || t.Topic.TopicName.Contains(searchString)
                                                                    || t.Trainer.TrainerName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "trainerName_desc":
                    trainer_Course_Topic = trainer_Course_Topic.OrderByDescending(t => t.Trainer.TrainerName);
                    break;
                case "courseName_desc":
                    trainer_Course_Topic = trainer_Course_Topic.OrderByDescending(t => t.Course.CourseName);
                    break;
                case "topicName_desc":
                    trainer_Course_Topic = trainer_Course_Topic.OrderByDescending(t => t.Topic.TopicName);
                    break;
                default:
                    trainer_Course_Topic = trainer_Course_Topic.OrderBy(t => t.Trainer.TrainerName);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(trainer_Course_Topic.ToPagedList(pageNumber, pageSize));
        }

        // GET: Trainer_Course_Topic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer_Course_Topic trainer_Course_Topic = db.Trainer_Course_Topic.Find(id);
            if (trainer_Course_Topic == null)
            {
                return HttpNotFound();
            }
            return View(trainer_Course_Topic);
        }

        // GET: Trainer_Course_Topic/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName");
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "TrainerUserID");
            return View();
        }

        // POST: Trainer_Course_Topic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrainerCourseTopicID,TrainerID,CourseID,TopicID")] Trainer_Course_Topic trainer_Course_Topic)
        {
            if (ModelState.IsValid)
            {
                db.Trainer_Course_Topic.Add(trainer_Course_Topic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", trainer_Course_Topic.CourseID);
            ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName", trainer_Course_Topic.TopicID);
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "TrainerUserID", trainer_Course_Topic.TrainerID);
            return View(trainer_Course_Topic);
        }

        // GET: Trainer_Course_Topic/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer_Course_Topic trainer_Course_Topic = db.Trainer_Course_Topic.Find(id);
            if (trainer_Course_Topic == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", trainer_Course_Topic.CourseID);
            ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName", trainer_Course_Topic.TopicID);
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "TrainerUserID", trainer_Course_Topic.TrainerID);
            return View(trainer_Course_Topic);
        }

        // POST: Trainer_Course_Topic/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrainerCourseTopicID,TrainerID,CourseID,TopicID")] Trainer_Course_Topic trainer_Course_Topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainer_Course_Topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", trainer_Course_Topic.CourseID);
            ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName", trainer_Course_Topic.TopicID);
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "TrainerUserID", trainer_Course_Topic.TrainerID);
            return View(trainer_Course_Topic);
        }

        // GET: Trainer_Course_Topic/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer_Course_Topic trainer_Course_Topic = db.Trainer_Course_Topic.Find(id);
            if (trainer_Course_Topic == null)
            {
                return HttpNotFound();
            }
            return View(trainer_Course_Topic);
        }

        // POST: Trainer_Course_Topic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trainer_Course_Topic trainer_Course_Topic = db.Trainer_Course_Topic.Find(id);
            db.Trainer_Course_Topic.Remove(trainer_Course_Topic);
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
