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
    [Authorize]
    public class TraineesController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        [Authorize(Roles = "Trainee,TrainingStaff,Admin,Trainer")]
        // GET: Trainees
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TraineeUserIDSortParm = String.IsNullOrEmpty(sortOrder) ? "traineeUserID_desc" : "";
            ViewBag.TraineeNameSortParm = String.IsNullOrEmpty(sortOrder) ? "traineeName_desc" : "";
            ViewBag.TraineeAccountSortParm = String.IsNullOrEmpty(sortOrder) ? "traineeAccount_desc" : "";
            ViewBag.DOBSortParm = sortOrder == "dob_asc" ? "dob_desc" : "dob_asc";
            ViewBag.EducationSortParm = String.IsNullOrEmpty(sortOrder) ? "education_desc" : "";
            ViewBag.MPLSortParm = String.IsNullOrEmpty(sortOrder) ? "mpl_desc" : "";
            ViewBag.TOEICSortParm = sortOrder == "toeic_asc" ? "toeic_desc" : "toeic_asc";
            ViewBag.ExDetailSortParm = String.IsNullOrEmpty(sortOrder) ? "exDetail_desc" : "";
            ViewBag.DepartmentSortParm = String.IsNullOrEmpty(sortOrder) ? "department_desc" : "";
            ViewBag.LocationSortParm = String.IsNullOrEmpty(sortOrder) ? "location_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var trainees = from t in db.Trainees
                           select t;
            if (!String.IsNullOrEmpty(searchString))
            {
                trainees = trainees.Where(t => t.TraineeUserID.Contains(searchString)
                                            || t.TraineeName.Contains(searchString)
                                            || t.TraineeAccount.Contains(searchString)
                                            || t.Education.Contains(searchString)
                                            || t.MPL.Contains(searchString)
                                            || t.ExDetail.Contains(searchString)
                                            || t.Department.Contains(searchString)
                                            || t.Location.Contains(searchString));
            }
            switch(sortOrder)
            {
                case "traineeUserID_desc":
                    trainees = trainees.OrderByDescending(t => t.TraineeUserID);
                    break;
                case "traineeName_desc":
                    trainees = trainees.OrderByDescending(t => t.TraineeName);
                    break;
                case "traineeAccount_desc":
                    trainees = trainees.OrderByDescending(t => t.TraineeAccount);
                    break;
                case "dob_asc":
                    trainees = trainees.OrderBy(t => t.DOB);
                    break;
                case "dob_desc":
                    trainees = trainees.OrderByDescending(t => t.DOB);
                    break;
                case "education_desc":
                    trainees = trainees.OrderByDescending(t => t.Education);
                    break;
                case "mpl_desc":
                    trainees = trainees.OrderByDescending(t => t.MPL);
                    break;
                case "toeic_asc":
                    trainees = trainees.OrderBy(t => t.TOEIC_score);
                    break;
                case "toeic_desc":
                    trainees = trainees.OrderByDescending(t => t.TOEIC_score);
                    break;
                case "exDetail_desc":
                    trainees = trainees.OrderByDescending(t => t.ExDetail);
                    break;
                case "department_desc":
                    trainees = trainees.OrderByDescending(t => t.Department);
                    break;
                case "location_desc":
                    trainees = trainees.OrderByDescending(t => t.Location);
                    break;
                default:
                    trainees = trainees.OrderBy(t => t.TraineeUserID);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(trainees.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Trainee,TrainingStaff,Admin")]
        // GET: Trainees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainee trainee = db.Trainees.Find(id);
            if (trainee == null)
            {
                return HttpNotFound();
            }
            return View(trainee);
        }

        [Authorize(Roles = "TrainingStaff,Admin")]
        // GET: Trainees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trainees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TraineeID,TraineeUserID,TraineeName,TraineeAccount,DOB,Education,MPL,TOEIC_score,ExDetail,Department,Location")] Trainee trainee)
        {
            if (ModelState.IsValid)
            {
                db.Trainees.Add(trainee);
                db.SaveChanges();

                AuthenController.CreateAccount(trainee.TraineeUserID, "123456", "Trainee");

                return RedirectToAction("Index");
            }

            return View(trainee);
        }

        [Authorize(Roles = "Trainee,TrainingStaff,Admin")]
        // GET: Trainees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainee trainee = db.Trainees.Find(id);
            if (trainee == null)
            {
                return HttpNotFound();
            }
            return View(trainee);
        }

        // POST: Trainees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TraineeID,TraineeUserID,TraineeName,TraineeAccount,DOB,Education,MPL,TOEIC_score,ExDetail,Department,Location")] Trainee trainee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainee);
        }

        [Authorize(Roles = "TrainingStaff,Admin")]
        // GET: Trainees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainee trainee = db.Trainees.Find(id);
            if (trainee == null)
            {
                return HttpNotFound();
            }
            return View(trainee);
        }

        // POST: Trainees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trainee trainee = db.Trainees.Find(id);
            db.Trainees.Remove(trainee);
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
