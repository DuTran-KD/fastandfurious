using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eTUTOR.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected void setAlert (string mes, string type)
        {
            TempData["AMess"] = mes;
            if(type == "success")
            {
                TempData["AType"] = "alert-success";
            }
            else if(type == "warning")
            {
                TempData["AType"] = "alert-warning";

            }else if(type == "error")
            {
                TempData["AType"] = "alert-danger";
            }
        }
    }
}