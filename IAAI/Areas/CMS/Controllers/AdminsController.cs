using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IAAI.Models;
using Newtonsoft.Json;

namespace IAAI.Areas.CMS.Controllers
{
    public class AdminsController : Controller
    {
        private IAAIDbContext _db = new IAAIDbContext();

        #region 新增管理者
        public ActionResult Create()
        {
            List<Permission> permissions = _db.Permissions.ToList();
            var roots = permissions.Where(p => p.RootId == null);
            var tree = GetNode(roots);
            ViewBag.tree = JsonConvert.SerializeObject(tree);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin admin)
        {
            if (ModelState.IsValid)
            {
                //檢查帳號有無重複
                admin.Account = admin.Account.ToLower();
                var isRegistered = _db.Admins.FirstOrDefault(u => u.Account == admin.Account);
                if (isRegistered == null)
                {
                    admin.Password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(admin.Password))).Replace("-", null);
                    admin.InitDate = DateTime.Now;
                    admin.Permission = "先寫死";
                    _db.Admins.Add(admin);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Tip = "此帳號已註冊";
                }
            }

            return View(admin);
        }
        #endregion


        #region 管理權限樹
        private object GetNode(IEnumerable<Permission> permissions)
        {
            return permissions.Select(permission => new
            {
                id = permission.Code,
                text = permission.Subject,
                children = permission.Permissions.Count > 0 ? GetNode(permission.Permissions) : null
            });
        }
        #endregion


        #region 管理者登入
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin login)
        {
            ModelState.Remove("UserName");

            if (ModelState.IsValid)
            {
                //檢查帳號有無重複
                login.Account = login.Account.ToLower();
                var admin = _db.Admins.FirstOrDefault(u => u.Account == login.Account);
                if (admin != null)
                {
                    login.Password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(login.Password))).Replace("-", null);

                    if (admin.Password == login.Password)
                    {
                        //宣告驗證票要夾帶的資料 (用;區隔)
                        string userData = admin.Account.ToString() + ";"
                                        + admin.UserName.ToString() + ";"
                                        + admin.ProfilePicture.ToString() + ";"
                                        + admin.Permission.ToString() + ";";
                        //設定驗證票(夾帶資料，cookie 命名)
                        SetAuthenTicket(userData, admin.Account);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Tip = "帳號或密碼有誤";
                    }
                }
                else
                {
                    ViewBag.Tip = "此帳號未註冊";
                }
            }

            return View(login);
        }
        #endregion


        #region 設定驗證票
        private void SetAuthenTicket(string userData, string userId)
        {
            //宣告一個驗證票
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddHours(12), false, userData);
            //加密驗證票
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //建立 Cookie
            HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //將 Cookie 寫入回應
            Response.Cookies.Add(authenticationCookie);
        }
        #endregion

















        // GET: CMS/Admins
        public ActionResult Index()
        {
            return View(_db.Admins.ToList());
        }

        // GET: CMS/Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = _db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "AdminId,Account,Password,UserName,ProfilePicture,InitDate,Permission")] Admin admin)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Admins.Add(admin);
        //        _db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(admin);
        //}

        // GET: CMS/Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = _db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: CMS/Admins/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminId,Account,Password,UserName,ProfilePicture,InitDate,Permission")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(admin).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: CMS/Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = _db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: CMS/Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = _db.Admins.Find(id);
            _db.Admins.Remove(admin);
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
