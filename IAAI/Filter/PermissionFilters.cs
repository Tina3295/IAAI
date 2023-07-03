using IAAI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace IAAI.Filter
{
    public class PermissionFilters : ActionFilterAttribute
    {
        private IAAIDbContext _db = new IAAIDbContext();


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var formsIdentity = HttpContext.Current.User.Identity as FormsIdentity;
            string[] user = formsIdentity.Ticket.UserData.Split(';');
            string permission = user[3];
            string[] allowedPermissions = permission.Split(',');

            //完整的tree
            List<Permission> permissions = _db.Permissions.ToList();
            var roots = permissions.Where(p => p.RootId == null);
            var menuData = GetNode(roots);

            //遞迴組字串
            StringBuilder sbMenu = new StringBuilder();
            BuildMenu(sbMenu, menuData, allowedPermissions);
            filterContext.Controller.ViewBag.menu = sbMenu.ToString();




            // 檢查是否具有訪問權限
            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RouteData.Values["action"].ToString();

            if (!HasPermission(controllerName, actionName, menuData, allowedPermissions))
            {
                // 沒有權限
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Unauthorized" }));
                return;
            }

            base.OnActionExecuting(filterContext);
        }


        //全部權限
        private object GetNode(IEnumerable<Permission> permissions)
        {
            return permissions.Select(permission => new
            {
                id = permission.Code,
                text = permission.Subject,
                url = permission.URL,
                children = permission.Permissions.Count > 0 ? GetNode(permission.Permissions) : null
            });
        }


        //第一層選單
        private void BuildMenu(StringBuilder sbMenu, dynamic menuData, string[] allowedPermissions)
        {
            foreach (var item in menuData)
            {
                if (allowedPermissions.Any(p => p.StartsWith(item.id)))
                {
                    sbMenu.AppendLine("<li class='nav-item'>");
                    sbMenu.AppendLine($"<a class='nav-link collapsed' data-bs-target='#{item.id}-nav' data-bs-toggle='collapse' href='#'>");
                    sbMenu.AppendLine($"<i class='bi bi-journal-text'></i><span>{item.text}</span><i class='bi bi-chevron-down ms-auto'></i>");
                    sbMenu.AppendLine("</a>");

                    if (item.children != null)
                    {
                        var children = item.children as IEnumerable<dynamic>;
                        if (children != null)
                        {
                            sbMenu.AppendLine($"<ul id='{item.id}-nav' class='nav-content collapse' data-bs-parent='#sidebar-nav'>");
                            BuildSubMenu(sbMenu, item.children, allowedPermissions);
                            sbMenu.AppendLine("</ul>");
                        }
                    }

                    sbMenu.AppendLine("</li>");
                }
            }
        }


        //第二層選單
        private void BuildSubMenu(StringBuilder sbMenu, IEnumerable<dynamic> menuData, string[] allowedPermissions)
        {
            foreach (var item in menuData)
            {
                if (allowedPermissions.Any(p => p.StartsWith(item.id)))
                {
                    sbMenu.AppendLine("<li>");
                    sbMenu.AppendLine($"<a href='/CMS{item.url}'>");
                    sbMenu.AppendLine($"<span>{item.text}</span>");
                    sbMenu.AppendLine("</a>");
                    sbMenu.AppendLine("</li>");
                }
            }
        }



        private bool HasPermission(string controller, string action, dynamic menuData, string[] allowedPermissions)
        {
            string requiredPermission = $"/{controller}/{action}";

            if (requiredPermission == "/Admins/LoginSuccess")
            {
                return true;
            }

            foreach (var item in menuData)
            {
                if (item.url == requiredPermission && allowedPermissions.Any(p => p.StartsWith(item.id)))
                {
                    return true;
                }
                else if (item.children != null)
                {
                    var children = item.children as IEnumerable<dynamic>;
                    if (children != null && HasPermission(controller, action, children, allowedPermissions))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}