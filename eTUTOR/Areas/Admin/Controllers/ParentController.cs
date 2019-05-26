using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;

namespace eTUTOR.Areas.Admin.Controllers
{
    public class ParentController : Controller
    {
        eTUITOREntities db = new eTUITOREntities();
        // GET: Admin/Parent
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(int id)
        {
            var parent = db.parents.Find(id);
            if (parent == null) return HttpNotFound();
            return View(parent);

        }
    }
}