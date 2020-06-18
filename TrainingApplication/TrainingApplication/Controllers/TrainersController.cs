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
    
    public class TrainersController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        [Authorize(Roles = "Trainer,TrainingStaff,Admin")]
        // GET: Trainers
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TrainerUserIDSortParm = String.IsNullOrEmpty(sortOrder) ? "trainerUserID_desc" : "";
            ViewBag.TrainerNameSortParm = String.IsNullOrEmpty(sortOrder) ? "trainerName_desc" : "";
            ViewBag.EducationSortParm = String.IsNullOrEmpty(sortOrder) ? "education_desc" : "";
            ViewBag.WorkingPlaceSortParm = String.IsNullOrEmpty(sortOrder) ? "workingPlace_desc" : "";
            ViewBag.TelephoneSortParm = sortOrder == "telephone_asc" ? "telephone_desc" : "telephone_asc";
            ViewBag.EmailSortParm = String.IsNullOrEmpty(sortOrder) ? "email_desc" : "";
            ViewBag.ExternalSortParm = String.IsNullOrEmpty(sortOrder) ? "external_desc" : "";
            ViewBag.InternalSortParm = String.IsNullOrEmpty(sortOrder) ? "internal_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var trainers = from t in db.Trainers
                           select t;
            if (!String.IsNullOrEmpty(searchString))
            {
                trainers = trainers.Where(t => t.TrainerUserID.Contains(searchString)
                                            || t.TrainerName.Contains(searchString)
                                            || t.Education.Contains(searchString)
                                            || t.WorkingPlace.Contains(searchString)
                                            || t.Email.Contains(searchString));                       
            }

            switch (sortOrder)
            {
                case "trainerUserID_desc":
                    trainers = trainers.OrderByDescending(t => t.TrainerUserID);
                    break;
                case "trainerName_desc":
                    trainers = trainers.OrderByDescending(t => t.TrainerName);
                    break;
                case "education_desc":
                    trainers = trainers.OrderByDescending(t => t.Education);
                    break;
                case "workingPlace_desc":
                    trainers = trainers.OrderByDescending(t => t.WorkingPlace);
                    break;
                case "telephone_asc":
                    trainers = trainers.OrderBy(t => t.Telephone);
                    break;
                case "telephone_desc":
                    trainers = trainers.OrderByDescending(t => t.Telephone);
                    break;
                case "email_desc":
                    trainers = trainers.OrderByDescending(t => t.Email);
                    break;
                case "external_desc":
                    trainers = trainers.OrderByDescending(t => t.External);
                    break;
                case "internal_desc":
                    trainers = trainers.OrderByDescending(t => t.Internal);
                    break;
                default:
                    trainers = trainers.OrderBy(t => t.TrainerUserID);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(trainers.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Trainer,TrainingStaff,Admin")]
        // GET: Trainers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = db.Trainers.Find(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }

        [Authorize(Roles = "TrainingStaff,Admin")]
        // GET: Trainers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trainers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrainerID,TrainerUserID,TrainerName,Education,WorkingPlace,Telephone,Email,External,Internal")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                db.Trainers.Add(trainer);
                db.SaveChanges();

                AuthenController.CreateAccount(trainer.TrainerUserID, "123456", "Trainer");

                return RedirectToAction("Index");
            }

            return View(trainer);
        }

        [Authorize(Roles = "Trainer,TrainingStaff,Admin")]
        // GET: Trainers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = db.Trainers.Find(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrainerID,TrainerUserID,TrainerName,Education,WorkingPlace,Telephone,Email,External,Internal")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainer);
        }

        [Authorize(Roles = "TrainingStaff,Admin")]
        // GET: Trainers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = db.Trainers.Find(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trainer trainer = db.Trainers.Find(id);
            db.Trainers.Remove(trainer);
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
