using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAAI.Models;

namespace IAAI.Areas.CMS.Controllers
{
    public class HomeController : Controller
    {
        #region 401
        public ActionResult UnAuthorized()
        {
            return View();
        }
        #endregion
    }
}
