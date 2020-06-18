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
    public class TrainingStaffsController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        [Authorize(Roles = "Admin,TrainingStaff")]
        // GET: TrainingStaffs
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TrainingStaffUserIDSortParm = String.IsNullOrEmpty(sortOrder) ? "trainingStaffUserID_desc" : "";
            ViewBag.TrainingStaffNameSortParm = String.IsNullOrEmpty(sortOrder) ? "trainingStaffName_desc" : "";
            ViewBag.TrainingStaffDesSortParm = String.IsNullOrEmpty(sortOrder) ? "trainingStaffDes_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var trainingStaffs = from t in db.TrainingStaffs
                                 select t;
            if(!String.IsNullOrEmpty(searchString))
            {
                trainingStaffs = trainingStaffs.Where(t => t.TrainingStaffUserID.Contains(searchString)
                                                        || t.TrainingStaffName.Contains(searchString)
                                                        || t.TrainingStaffDes.Contains(searchString));
            }
            
            switch (sortOrder)
            {
                case "trainingStaffUserID_desc":
                    trainingStaffs = trainingStaffs.OrderByDescending(t => t.TrainingStaffUserID);
                    break;
                case "trainingStaffName_desc":
                    trainingStaffs = trainingStaffs.OrderByDescending(t => t.TrainingStaffName);
                    break;
                case "trainingStaffDes_desc":
                    trainingStaffs = trainingStaffs.OrderByDescending(t => t.TrainingStaffDes);
                    break;
                default:
                    trainingStaffs = trainingStaffs.OrderBy(t => t.TrainingStaffUserID);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(trainingStaffs.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Admin,TrainingStaff")]
        // GET: TrainingStaffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainingStaff trainingStaff = db.TrainingStaffs.Find(id);
            if (trainingStaff == null)
            {
                return HttpNotFound();
            }
            return View(trainingStaff);
        }

        [Authorize(Roles = "Admin")]
        // GET: TrainingStaffs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainingStaffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrainingStaffID,TrainingStaffUserID,TrainingStaffName,TrainingStaffDes")] TrainingStaff trainingStaff)
        {
            if (ModelState.IsValid)
            {
                db.TrainingStaffs.Add(trainingStaff);
                db.SaveChanges();

                AuthenController.CreateAccount(trainingStaff.TrainingStaffUserID, "123456", "TrainingStaff");

                return RedirectToAction("Index");
            }

            return View(trainingStaff);
        }

        [Authorize(Roles = "Admin,TrainingStaff")]
        // GET: TrainingStaffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainingStaff trainingStaff = db.TrainingStaffs.Find(id);
            if (trainingStaff == null)
            {
                return HttpNotFound();
            }
            return View(trainingStaff);
        }

        // POST: TrainingStaffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrainingStaffID,TrainingStaffUserID,TrainingStaffName,TrainingStaffDes")] TrainingStaff trainingStaff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainingStaff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainingStaff);
        }

        [Authorize(Roles = "Admin")]
        // GET: TrainingStaffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainingStaff trainingStaff = db.TrainingStaffs.Find(id);
            if (trainingStaff == null)
            {
                return HttpNotFound();
            }
            return View(trainingStaff);
        }

        // POST: TrainingStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrainingStaff trainingStaff = db.TrainingStaffs.Find(id);
            db.TrainingStaffs.Remove(trainingStaff);
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
