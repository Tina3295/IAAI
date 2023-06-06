using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ganss.Xss;
using IAAI.Models;

namespace IAAI.Controllers
{
    public class ForumsController : Controller
    {
        private IAAIDbContext db = new IAAIDbContext();

        // GET: Forums
        public ActionResult Index()
        {
            var forums = db.Forums.Include(f => f.ForumMember);
            return View(forums.ToList());
        }

        // GET: Forums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forums forums = db.Forums.Find(id);
            if (forums == null)
            {
                return HttpNotFound();
            }
            return View(forums);
        }

        // GET: Forums/Create
        public ActionResult Create()
        {
            Forums model = new Forums();
            model.ForumMemberId = (int)Session["Id"];

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Forums forums)
        {
            if (string.IsNullOrWhiteSpace(forums.ContentHtml))
            {
                ViewBag.Tip = "內容必填";
            }
            if (ModelState.IsValid)
            {
                var sanitizer = new HtmlSanitizer();
                forums.ContentHtml = sanitizer.Sanitize(forums.ContentHtml);
                forums.InitDate = DateTime.Now;

                db.Forums.Add(forums);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(forums);
        }

        // GET: Forums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forums forums = db.Forums.Find(id);
            if (forums == null)
            {
                return HttpNotFound();
            }
            ViewBag.ForumMemberId = new SelectList(db.ForumMembers, "ForumMemberId", "Account", forums.ForumMemberId);
            return View(forums);
        }

        // POST: Forums/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ForumId,Title,ContentHtml,ForumMemberId,InitDate")] Forums forums)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forums).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ForumMemberId = new SelectList(db.ForumMembers, "ForumMemberId", "Account", forums.ForumMemberId);
            return View(forums);
        }

        // GET: Forums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forums forums = db.Forums.Find(id);
            if (forums == null)
            {
                return HttpNotFound();
            }
            return View(forums);
        }

        // POST: Forums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Forums forums = db.Forums.Find(id);
            db.Forums.Remove(forums);
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
