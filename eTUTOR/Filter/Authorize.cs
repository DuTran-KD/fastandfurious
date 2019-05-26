using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using eTUTOR.Controllers;
namespace eTUTOR.Filter
{
    public class Authorize : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (HttpContext.Current.Session["UserID"] == null)
            {
                FormsAuthentication.SignOut();
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller","User"},
                    {"Action","Login" },
                    {"returnUrl", filterContext.HttpContext.Request.RawUrl}
                });

                return;
            }
            base.OnActionExecuted(filterContext);
        }
        

    }
}