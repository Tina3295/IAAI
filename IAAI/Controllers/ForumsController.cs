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
using static IAAI.Models.ViewModel;
using MvcPaging;

namespace IAAI.Controllers
{
    public class ForumsController : Controller
    {
        private IAAIDbContext _db = new IAAIDbContext();
        private const int DefaultPageSize = 2;

        // GET: Forums
        public ActionResult Index(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            ViewBag.Count = _db.Forums.Count();

            var forums = _db.Forums.ToList().Select(f =>
            {
                var latestReply = _db.ForumReplies.Where(r => r.ForumId == f.ForumId).OrderByDescending(r=>r.InitDate).FirstOrDefault();
                var forumMember = latestReply != null ? _db.ForumMembers.FirstOrDefault(m => m.ForumMemberId == latestReply.ForumMemberId) : null;

                return new ForumIndex
                {
                    ForumId = f.ForumId,
                    Title = f.Title,
                    Author = f.ForumMember.Name,
                    InitDate = f.InitDate?.ToString("yyyy/MM/dd"),
                    LatestResponder = forumMember?.Name,
                    LatestInitDate = latestReply?.InitDate?.ToString("yyyy/MM/dd"),
                    RepliesCount = _db.ForumReplies.Count(r => r.ForumId == f.ForumId)
                };
            });

            return View(forums.OrderByDescending(p => p.InitDate).ToPagedList(currentPageIndex, DefaultPageSize));
        }

        // GET: Forums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forums forums = _db.Forums.Find(id);
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

                _db.Forums.Add(forums);
                _db.SaveChanges();
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
            Forums forums = _db.Forums.Find(id);
            if (forums == null)
            {
                return HttpNotFound();
            }
            ViewBag.ForumMemberId = new SelectList(_db.ForumMembers, "ForumMemberId", "Account", forums.ForumMemberId);
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
                _db.Entry(forums).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ForumMemberId = new SelectList(_db.ForumMembers, "ForumMemberId", "Account", forums.ForumMemberId);
            return View(forums);
        }

        // GET: Forums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forums forums = _db.Forums.Find(id);
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
            Forums forums = _db.Forums.Find(id);
            _db.Forums.Remove(forums);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
