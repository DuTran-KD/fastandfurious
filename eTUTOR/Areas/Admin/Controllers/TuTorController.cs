using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
namespace eTUTOR.Areas.Admin.Controllers
{
    public class TuTorController : Controller
    {
        eTUITOREntities db = new eTUITOREntities();
        // GET: Admin/TuTor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(int id)
        {
            var tutor = db.tutors.Find(id);
            if (tutor == null) return HttpNotFound();
            return View(tutor);

        }
    }
}