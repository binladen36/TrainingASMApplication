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
    public class TrainerActionController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();
        // GET: TrainerAction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TrainerProfile()
        {
            var userName = User.Identity.Name;

            var trainer = (from t in db.Trainers
                           where t.TrainerUserID.Equals(userName)
                           select t).FirstOrDefault();
            return View(trainer);
        }

        public ActionResult TrainerCourseTopic()
        {
            var userName = User.Identity.Name;

            var trainer = (from t in db.Trainer_Course_Topic
                           where t.Trainer.TrainerUserID.Equals(userName)
                           select t);
            return View(trainer.ToList());
        }

        [Authorize(Roles = "Trainer")]
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
    }
}