using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainingApplication.Models;

namespace TrainingApplication.Controllers
{
    public class TraineeActionController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();
        // GET: TraineeAction
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult TraineeProfile()
        {
            var userName = User.Identity.Name;

            var trainee = (from t in db.Trainees
                           where t.TraineeUserID.Equals(userName)
                           select t).FirstOrDefault();
            return View(trainee);
        }

        public ViewResult TraineeCourse()
        {
            var userName = User.Identity.Name;

            var trainningManagings = (from t in db.Trainee_Course
                                      where t.Trainee.TraineeUserID.Equals(userName)
                                      select t);
            return View(trainningManagings);
        }

        [Authorize(Roles = "Trainee")]
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
    }
}