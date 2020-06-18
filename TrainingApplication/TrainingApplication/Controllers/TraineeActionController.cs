using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult TraineeProfile()
        {
            var userName = User.Identity.Name;

            var trainer = (from t in db.Trainees
                           where t.TraineeUserID.Equals(userName)
                           select t).FirstOrDefault();
            return View(trainer);
        }

        public ActionResult TraineeCourse()
        {
            var userName = User.Identity.Name;

            var trainningManagings = (from t in db.TrainningManagings
                                      where t.Trainee.TraineeAccount.Equals(userName)
                                      select t);
            return View(trainningManagings.ToList());
        }
    }
}