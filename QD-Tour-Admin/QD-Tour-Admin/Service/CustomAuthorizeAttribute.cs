using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Admin.Service
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var admin = filterContext.HttpContext.Session["adminLogedIn"];
            if (admin == null)
                filterContext.Result = new RedirectResult(string.Format("/Login/Index", filterContext.HttpContext.Request.Url.AbsolutePath));
        }
    }
}