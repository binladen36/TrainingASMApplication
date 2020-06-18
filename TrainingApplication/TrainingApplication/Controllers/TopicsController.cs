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
    public class TopicsController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        [Authorize(Roles = "Trainer,TrainingStaff")]
        // GET: Topics
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TopicNameSortParm = String.IsNullOrEmpty(sortOrder) ? "topicName_desc" : "";
            ViewBag.TopicDesSortParm = String.IsNullOrEmpty(sortOrder) ? "topicDes_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var topics = from t in db.Topics
                         select t;
            if (!String.IsNullOrEmpty(searchString))
            {
                topics = topics.Where(t => t.TopicName.Contains(searchString)
                                        || t.TopicDescription.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "topicName_desc":
                    topics = topics.OrderByDescending(t => t.TopicName);
                    break;
                case "topicDes_desc":
                    topics = topics.OrderByDescending(t => t.TopicDescription);
                    break;
                default:
                    topics = topics.OrderBy(t => t.TopicName);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(topics.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "TrainingStaff,Trainer")]
        // GET: Topics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        [Authorize(Roles = "TrainingStaff")]
        // GET: Topics/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TopicID,TopicName,TopicDescription")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Topics.Add(topic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(topic);
        }

        [Authorize(Roles = "TrainingStaff")]
        // GET: Topics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicID,TopicName,TopicDescription")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        [Authorize(Roles = "TrainingStaff")]
        // GET: Topics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topic topic = db.Topics.Find(id);
            db.Topics.Remove(topic);
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
