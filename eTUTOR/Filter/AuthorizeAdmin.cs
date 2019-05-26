using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eTUTOR.Filter
{
    public class AuthorizeAdmin : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (
                HttpContext.Current.Session["UserID"] == null || HttpContext.Current.Session["isAdmin"] == null)
            {
                filterContext.Result = new HttpStatusCodeResult(404);

                return;
            }
            base.OnActionExecuted(filterContext);
        }
    }
}