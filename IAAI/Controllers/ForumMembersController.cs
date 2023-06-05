using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using GoogleRecaptcha;
using IAAI.Models;
using static IAAI.Models.ViewModel;

namespace IAAI.Controllers
{
    public class ForumMembersController : Controller
    {
        private IAAIDbContext _db = new IAAIDbContext();

        //註冊討論區會員
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(ForumMember forumMember, string confirmPassword, string IsMembership, HttpPostedFileBase upfile)
        {
            string savePath = Server.MapPath("~/Upload/Membership/");

            //google驗證
            IRecaptcha<RecaptchaV2Result> recaptcha = new RecaptchaV2(new RecaptchaV2Data()
            {
                Secret = WebConfigurationManager.AppSettings["SecretKey"]
            });
            var result = recaptcha.Verify();

            if (ModelState.IsValid)
            {
                //檢查帳號有無重複
                forumMember.Account = forumMember.Account.ToLower();
                var isRegistered = _db.ForumMembers.FirstOrDefault(u => u.Account == forumMember.Account);
                if (isRegistered == null)
                {
                    //確認密碼與密碼是否相符
                    if (forumMember.Password != confirmPassword)
                    {
                        ModelState.AddModelError("confirmPassword", "確認密碼與密碼不相符");
                        return View(forumMember);
                    }

                    //是否為當年度有效會員
                    //當 checkbox 被勾選時，其值會被設定為字串 "on"
                    if (IsMembership == "on" && upfile != null && upfile.ContentLength > 0)
                    {
                        string fileName = upfile.FileName;
                        string extension = Path.GetExtension(fileName);

                        //檢查非圖片檔
                        if (extension != ".jpg" && extension != ".png" && extension != ".gif" && extension != ".jpeg")
                        {
                            ModelState.AddModelError("IsMembership", fileName + " 非圖片檔!");
                            return View(forumMember);
                        }

                        //存檔(重新命名)
                        forumMember.Membership = Guid.NewGuid().ToString() + forumMember.Account + extension;
                        var path = Path.Combine(savePath, forumMember.Membership);
                        upfile.SaveAs(path);
                    }
                    else if (IsMembership == "on")
                    {
                        ModelState.AddModelError("IsMembership", "未檢附會員證影本");
                    }
                    else if (upfile != null && upfile.ContentLength > 0)
                    {
                        ModelState.AddModelError("IsMembership", "未勾選，檔案上傳失敗");
                    }

                    //檢查google驗證
                    if (!result.Success)
                    {
                        ModelState.AddModelError("IsVerify", "驗證失敗，請重新驗證");
                    }

                    //檢查服務經歷資料
                    CheckExperience(forumMember.HistoryUnit1, forumMember.HistoryJobTitle1, forumMember.StartYear1, forumMember.StartMonth1, forumMember.EndYear1, forumMember.EndMonth1, "Experience1");
                    CheckExperience(forumMember.HistoryUnit2, forumMember.HistoryJobTitle2, forumMember.StartYear2, forumMember.StartMonth2, forumMember.EndYear2, forumMember.EndMonth2, "Experience2");
                    CheckExperience(forumMember.HistoryUnit3, forumMember.HistoryJobTitle3, forumMember.StartYear3, forumMember.StartMonth3, forumMember.EndYear3, forumMember.EndMonth3, "Experience3");

                    if (!ModelState.IsValid)
                    {
                        return View(forumMember);
                    }

                    forumMember.Password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(forumMember.Password))).Replace("-", null);
                    forumMember.InitDate = DateTime.Now;
                    _db.ForumMembers.Add(forumMember);
                    _db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Tip = "此帳號已註冊";
                }
            }

            return View(forumMember);
        }

        // 判斷服務經歷資料是否完整
        private void CheckExperience(string unit, string jobTitle, int? startYear, int? startMonth, int? endYear, int? endMonth, string errorMessageKey)
        {
            if (!string.IsNullOrWhiteSpace(unit) ||
                !string.IsNullOrWhiteSpace(jobTitle) ||
                startYear != null ||
                startMonth != null ||
                endYear != null ||
                endMonth != null)
            {
                if (string.IsNullOrWhiteSpace(unit) ||
                    string.IsNullOrWhiteSpace(jobTitle) ||
                    startYear == null ||
                    startMonth == null ||
                    endYear == null ||
                    endMonth == null)
                {
                    ModelState.AddModelError(errorMessageKey, "請輸入完整的一組資料");
                }
                else
                {
                    DateTime startDateTime = new DateTime(startYear.Value, startMonth.Value, 1);
                    DateTime endDateTime = new DateTime(endYear.Value, endMonth.Value, 1);

                    if (startDateTime > endDateTime)
                    {
                        ModelState.AddModelError(errorMessageKey, "起始年月必須小於結束年月");
                    }
                }
            }
        }


        //討論區會員登入
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(ForumLogin view)
        {
            ModelState.Remove("UserName");

            if (ModelState.IsValid)
            {
                //檢查帳號有無重複
                view.Account = view.Account.ToLower();
                var user = _db.ForumMembers.FirstOrDefault(u => u.Account == view.Account);
                if (user != null)
                {
                    view.Password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(view.Password))).Replace("-", null);

                    if (user.Password == view.Password)
                    {
                        Session["Id"] = user.ForumMemberId;
                        Session["Name"] = user.Name;
                        Session["Account"] = user.Account;

                        return RedirectToAction("Index", "Forums");
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

            return View(view);
        }




        // 編輯會員資訊
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumMember forumMember = _db.ForumMembers.Find(id);
            if (forumMember == null)
            {
                return HttpNotFound();
            }

            //是否有國際會籍
            ViewBag.IsMembership = false;
            if (forumMember.Membership != null)
            {
                ViewBag.IsMembership = true;
                ViewBag.MembershipDownload = "<a href='" + Url.Content("~/Upload/Membership/" + forumMember.Membership) + "' download>下載會員證影本</a>";
            }

            //計算合計年資
            int totalMonths = 0;

            if (forumMember.HistoryUnit1 != null)
            {
                totalMonths += ExpYearMonth((int)forumMember.StartYear1, (int)forumMember.StartMonth1, (int)forumMember.EndYear1, (int)forumMember.EndMonth1);
            }
            if (forumMember.HistoryUnit2 != null)
            {
                totalMonths += ExpYearMonth((int)forumMember.StartYear2, (int)forumMember.StartMonth2, (int)forumMember.EndYear2, (int)forumMember.EndMonth2);
            }
            if (forumMember.HistoryUnit3 != null)
            {
                totalMonths += ExpYearMonth((int)forumMember.StartYear3, (int)forumMember.StartMonth3, (int)forumMember.EndYear3, (int)forumMember.EndMonth3);
            }

            ViewBag.TotalYear = totalMonths / 12;
            ViewBag.TotalMonth = totalMonths % 12;

            return View(forumMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ForumMember forumMember, string newPassword, string IsMembership, HttpPostedFileBase upfile)
        {
            string savePath = Server.MapPath("~/Upload/Membership/");

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    if (newPassword.Length < 8 || newPassword.Length > 100)
                    {
                        ViewBag.Password = "密碼長度需介於8~100個字元間";
                        return View(forumMember);
                    }
                    forumMember.Password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(newPassword))).Replace("-", null);
                }

                //變更國際會籍
                if (IsMembership == "on" && upfile != null && upfile.ContentLength > 0)
                {
                    string fileName = upfile.FileName;
                    string extension = Path.GetExtension(fileName);

                    //檢查非圖片檔
                    if (extension != ".jpg" && extension != ".png" && extension != ".gif" && extension != ".jpeg")
                    {
                        ModelState.AddModelError("IsMembership", fileName + " 非圖片檔!");
                        return View(forumMember);
                    }

                    //存檔(重新命名)
                    forumMember.Membership = Guid.NewGuid().ToString() + forumMember.Account + extension;
                    var path = Path.Combine(savePath, forumMember.Membership);
                    upfile.SaveAs(path);
                }
                else if (IsMembership == "on" && forumMember.Membership == null)
                {
                    ModelState.AddModelError("IsMembership", "未檢附會員證影本");
                }
                else if (upfile != null && upfile.ContentLength > 0)
                {
                    ModelState.AddModelError("IsMembership", "未勾選，檔案上傳失敗");
                }

                //檢查服務經歷資料
                CheckExperience(forumMember.HistoryUnit1, forumMember.HistoryJobTitle1, forumMember.StartYear1, forumMember.StartMonth1, forumMember.EndYear1, forumMember.EndMonth1, "Experience1");
                CheckExperience(forumMember.HistoryUnit2, forumMember.HistoryJobTitle2, forumMember.StartYear2, forumMember.StartMonth2, forumMember.EndYear2, forumMember.EndMonth2, "Experience2");
                CheckExperience(forumMember.HistoryUnit3, forumMember.HistoryJobTitle3, forumMember.StartYear3, forumMember.StartMonth3, forumMember.EndYear3, forumMember.EndMonth3, "Experience3");

                if (!ModelState.IsValid)
                {
                    return View(forumMember);
                }

                _db.Entry(forumMember).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Edit");
            }
            return View(forumMember);
        }

        private int ExpYearMonth(int startYear, int startMonth, int endYear, int endMonth)
        {
            DateTime startDateTime = new DateTime(startYear, startMonth, 1);
            DateTime endDateTime = new DateTime(endYear, endMonth, 1);

            TimeSpan timeDifference = endDateTime.Subtract(startDateTime);
            int differenceInMonths = timeDifference.Days / 30;

            return differenceInMonths;
        }








        // GET: ForumMembers
        public ActionResult Index()
        {
            return View(_db.ForumMembers.ToList());
        }




        // GET: ForumMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumMember forumMember = _db.ForumMembers.Find(id);
            if (forumMember == null)
            {
                return HttpNotFound();
            }
            return View(forumMember);
        }

        // POST: ForumMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ForumMember forumMember = _db.ForumMembers.Find(id);
            _db.ForumMembers.Remove(forumMember);
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
