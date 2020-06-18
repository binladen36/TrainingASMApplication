using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingApplication.Models;

namespace TrainingApplication.Controllers
{
    public class TrainingStaffActionController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();
        // GET: TrainingStaff
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TrainingStaffProfile()
        {
            var userName = User.Identity.Name;

            var trainingStaff = (from t in db.TrainingStaffs
                           where t.TrainingStaffUserID.Equals(userName)
                           select t).FirstOrDefault();
            return View(trainingStaff);
        }
    }
}