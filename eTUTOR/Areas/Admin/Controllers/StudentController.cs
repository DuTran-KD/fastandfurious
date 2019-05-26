using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
namespace eTUTOR.Areas.Admin.Controllers
{
    public class StudentController : Controller
    {
        eTUITOREntities db = new eTUITOREntities();
        // GET: Admin/Student
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(int id)
        {
            var student = db.students.Find(id);
            if (student == null) return HttpNotFound();
            return View(student);

        }
    }
}