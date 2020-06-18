using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult TrainerCourse()
        {
            var userName = User.Identity.Name;

            var trainningManagings = (from t in db.TrainningManagings
                           where t.Trainer.TrainerUserID.Equals(userName)
                           select t);
            return View(trainningManagings.ToList());
        }

        public ActionResult TrainerTopic()
        {
            var userName = User.Identity.Name;

            var trainningManagings = (from t in db.TrainningManagings
                                      where t.Topic.TopicID.Equals(userName)
                                      select t);
            return View(trainningManagings.ToList());
        }
    }
}