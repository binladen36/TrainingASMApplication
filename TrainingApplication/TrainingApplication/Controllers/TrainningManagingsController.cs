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
    public class TrainningManagingsController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        // GET: TrainningManagings
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TrainingStaffUserIDSortParm = String.IsNullOrEmpty(sortOrder) ? "trainingStaffUserID_desc" : "";
            ViewBag.TraineeUserIDSortParm = String.IsNullOrEmpty(sortOrder) ? "traineeUserID_desc" : "";
            ViewBag.CourseCateNameSortParm = String.IsNullOrEmpty(sortOrder) ? "courseCateName_desc" : "";
            ViewBag.CourseNameSortParm = String.IsNullOrEmpty(sortOrder) ? "courseName_desc" : "";
            ViewBag.TopicNameSortParm = String.IsNullOrEmpty(sortOrder) ? "topic_desc" : "";
            ViewBag.TrainerUserIDSortParm = String.IsNullOrEmpty(sortOrder) ? "trainerUserID_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var trainningManagings = db.TrainningManagings.Include(t => t.Course).Include(t => t.CourseCate).Include(t => t.Topic).Include(t => t.Trainee).Include(t => t.Trainer).Include(t => t.TrainingStaff);
            if(!String.IsNullOrEmpty(searchString))
            {
                trainningManagings = trainningManagings.Where(t => t.TrainingStaff.TrainingStaffUserID.Contains(searchString)
                                                                || t.Trainee.TraineeUserID.Contains(searchString)
                                                                || t.CourseCate.CourseCateName.Contains(searchString)
                                                                || t.Course.CourseName.Contains(searchString)
                                                                || t.Topic.TopicName.Contains(searchString)
                                                                || t.Trainer.TrainerUserID.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "trainingStaffUserID_desc":
                    trainningManagings = trainningManagings.OrderByDescending(t => t.TrainingStaff.TrainingStaffUserID);
                    break;
                case "traineeUserID_desc":
                    trainningManagings = trainningManagings.OrderByDescending(t => t.Trainee.TraineeUserID);
                    break;
                case "courseCateName_desc":
                    trainningManagings = trainningManagings.OrderByDescending(t => t.CourseCate.CourseCateName);
                    break;
                case "courseName_desc":
                    trainningManagings = trainningManagings.OrderByDescending(t => t.Course.CourseName);
                    break;
                case "topic_desc":
                    trainningManagings = trainningManagings.OrderByDescending(t => t.Topic.TopicName);
                    break;
                case "trainerUserID_desc":
                    trainningManagings = trainningManagings.OrderByDescending(t => t.Trainer.TrainerUserID);
                    break;
                default:
                    trainningManagings = trainningManagings.OrderBy(t => t.TrainingStaff.TrainingStaffUserID);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(trainningManagings.ToPagedList(pageNumber, pageSize));
        }

        // GET: TrainningManagings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainningManaging trainningManaging = db.TrainningManagings.Find(id);
            if (trainningManaging == null)
            {
                return HttpNotFound();
            }
            return View(trainningManaging);
        }

        // GET: TrainningManagings/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.CourseCateID = new SelectList(db.CourseCates, "CourseCateID", "CourseCateName");
            ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName");
            ViewBag.TraineeID = new SelectList(db.Trainees, "TraineeID", "TraineeUserID");
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "TrainerUserID");
            ViewBag.TrainingStaffID = new SelectList(db.TrainingStaffs, "TrainingStaffID", "TrainingStaffUserID");
            return View();
        }

        // POST: TrainningManagings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrainingManagingID,TrainingStaffID,TraineeID,CourseCateID,CourseID,TopicID,TrainerID")] TrainningManaging trainningManaging)
        {
            if (ModelState.IsValid)
            {
                db.TrainningManagings.Add(trainningManaging);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", trainningManaging.CourseID);
            ViewBag.CourseCateID = new SelectList(db.CourseCates, "CourseCateID", "CourseCateName", trainningManaging.CourseCateID);
            ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName", trainningManaging.TopicID);
            ViewBag.TraineeID = new SelectList(db.Trainees, "TraineeID", "TraineeUserID", trainningManaging.TraineeID);
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "TrainerUserID", trainningManaging.TrainerID);
            ViewBag.TrainingStaffID = new SelectList(db.TrainingStaffs, "TrainingStaffID", "TrainingStaffUserID", trainningManaging.TrainingStaffID);
            return View(trainningManaging);
        }

        // GET: TrainningManagings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainningManaging trainningManaging = db.TrainningManagings.Find(id);
            if (trainningManaging == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", trainningManaging.CourseID);
            ViewBag.CourseCateID = new SelectList(db.CourseCates, "CourseCateID", "CourseCateName", trainningManaging.CourseCateID);
            ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName", trainningManaging.TopicID);
            ViewBag.TraineeID = new SelectList(db.Trainees, "TraineeID", "TraineeUserID", trainningManaging.TraineeID);
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "TrainerUserID", trainningManaging.TrainerID);
            ViewBag.TrainingStaffID = new SelectList(db.TrainingStaffs, "TrainingStaffID", "TrainingStaffUserID", trainningManaging.TrainingStaffID);
            return View(trainningManaging);
        }

        // POST: TrainningManagings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrainingManagingID,TrainingStaffID,TraineeID,CourseCateID,CourseID,TopicID,TrainerID")] TrainningManaging trainningManaging)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainningManaging).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", trainningManaging.CourseID);
            ViewBag.CourseCateID = new SelectList(db.CourseCates, "CourseCateID", "CourseCateName", trainningManaging.CourseCateID);
            ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName", trainningManaging.TopicID);
            ViewBag.TraineeID = new SelectList(db.Trainees, "TraineeID", "TraineeUserID", trainningManaging.TraineeID);
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "TrainerUserID", trainningManaging.TrainerID);
            ViewBag.TrainingStaffID = new SelectList(db.TrainingStaffs, "TrainingStaffID", "TrainingStaffUserID", trainningManaging.TrainingStaffID);
            return View(trainningManaging);
        }

        // GET: TrainningManagings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainningManaging trainningManaging = db.TrainningManagings.Find(id);
            if (trainningManaging == null)
            {
                return HttpNotFound();
            }
            return View(trainningManaging);
        }

        // POST: TrainningManagings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrainningManaging trainningManaging = db.TrainningManagings.Find(id);
            db.TrainningManagings.Remove(trainningManaging);
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
