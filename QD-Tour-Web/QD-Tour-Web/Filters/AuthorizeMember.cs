using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QD_Tour_Web.Filters
{
    public class AuthorizeMember : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.Session["logedIn"];
            if (user == null)
                filterContext.Result = new RedirectResult(string.Format("/Login/Index", filterContext.HttpContext.Request.Url.AbsolutePath));
        }
    }
}