using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnipchenkoASPNETExam
{
    class AuthFilterAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsAuth = false;

            var authCookie = filterContext.HttpContext.Request.Cookies[Constants.AUTH_KEY];

            if (authCookie == null)
            {
                filterContext.Result = new HttpNotFoundResult();
            }
            else if (authCookie.Value != Constants.TRUE_VALUE)
            {
                filterContext.Result = new HttpNotFoundResult();
            }

            filterContext.Controller.ViewBag.IsAuth = true;
        }
    }
}