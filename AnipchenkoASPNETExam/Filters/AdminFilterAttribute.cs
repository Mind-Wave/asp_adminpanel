using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnipchenkoASPNETExam
{
    class AdminFilterAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsAdmin = false;

            HttpCookie adminCookie = filterContext.HttpContext.Request.Cookies[Constants.ADMIN_KEY];

            if (adminCookie == null)
            {
                filterContext.Result = new HttpNotFoundResult();
            }
            else if (adminCookie.Value != Constants.TRUE_VALUE)
            {
                filterContext.Result = new HttpNotFoundResult();
            }

            filterContext.Controller.ViewBag.IsAdmin = true;
        }
    }
}