using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
using eTUTOR.Service;
using System.Web.Helpers;

namespace eTUTOR.Areas.Admin.Controllers
{
    
    public class AdminController : Controller
    {
        eTUITOREntities model = new eTUITOREntities();
        CommonService commonService = new CommonService();
        // GET: Admin/Admin
        public ActionResult Login()
        {
            ViewBag.Message = "Login.";
            return View();
        }
        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Login(string email, string password)
        {
            var admin = model.admins.FirstOrDefault(x => x.email == email);
            
            if (admin != null)
            {
                string pass = commonService.hash(password);
                Session.Timeout = 30;
                if (admin.password.Equals(pass))
                {
                    Session["FullName"] = admin.fullname;
                    Session["UserID"] = admin.admin_id;

                    Session["isAdmin"] = "Admin";
                    return RedirectToAction("Dashboard", "Admin");
                }
            }
            else
            {
                ViewBag.mgs = "tài khoản không tồn tại";
            }
            return View();
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            return View("Login","Admin");
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Course()
        {
            if(Session["UserID"] == null) { return RedirectToAction("Login"); }
            var tutor_id = int.Parse(Session["UserID"].ToString());
            var info = model.tutors.FirstOrDefault();
            List<session> sessionList = model.sessions.Where(x => x.status_tutor == 1 && x.status_admin == 2).ToList();
            ViewData["sessionlist"] = sessionList;
            return View(info);
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Schedule()
        {
            var tutor_id = int.Parse(Session["UserID"].ToString());
            var schedule = model.schedules.Where(x => x.status == 2).ToList();
            return View(schedule);
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Dashboard()
        {
            ViewBag.Message = "Dashboard.";
            return View();
        }
        [Filter.AuthorizeAdmin]
        public ActionResult DetailParent(int id)
        {
            var parent = model.parents.Find(id);
            if (parent == null) return HttpNotFound();
            return View(parent);
        }
        [Filter.AuthorizeAdmin]
        public ActionResult DetailStudent(int id)
        {
            var student = model.students.Find(id);
            if (student == null) return HttpNotFound();
            return View(student);
        }
        [Filter.AuthorizeAdmin]
        public ActionResult DetailTutor(int id)
        {
            var tutor = model.tutors.Find(id);
            if (tutor == null) return HttpNotFound();
            return View(tutor);
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Detail()
        {
            ViewBag.Message = "Detail.";
            return View();
        }
        [Filter.AuthorizeAdmin]
        public ActionResult ThongKe()
        {
            return View();
        }
        
        [HttpGet]
        [Filter.AuthorizeAdmin]
        public ActionResult ThongKee(string kieuTK, DateTime dateStart, DateTime dateEnd)
        {
            if (kieuTK == "slDangKy")
            {
            }
            return View();
        }
        [Filter.AuthorizeAdmin]
        public ActionResult tkDangky()
        {
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var result = model.Users.ToList();
            decimal y1 = model.Users.Count(x => x.dateCreate.Value.Month == 1);
            decimal y2 = model.Users.Count(x => x.dateCreate.Value.Month == 2);
            decimal y3 = model.Users.Count(x => x.dateCreate.Value.Month == 3);
            decimal y4 = model.Users.Count(x => x.dateCreate.Value.Month == 4);
            decimal y5 = model.Users.Count(x => x.dateCreate.Value.Month == 5);
            decimal y6 = model.Users.Count(x => x.dateCreate.Value.Month == 6);
            decimal y7 = model.Users.Count(x => x.dateCreate.Value.Month == 7);
            decimal y8 = model.Users.Count(x => x.dateCreate.Value.Month == 8);
            decimal y9 = model.Users.Count(x => x.dateCreate.Value.Month == 9);
            decimal y10 = model.Users.Count(x => x.dateCreate.Value.Month == 10);
            decimal y11 = model.Users.Count(x => x.dateCreate.Value.Month == 11);
            decimal y12 = model.Users.Count(x => x.dateCreate.Value.Month == 12);

            //for(int i = 0; i < 12; i++)
            //{
            //    int u = i + 1;
            //    xValue[i] = "Tháng " + u;
            //}
             
            new Chart(width: 800, height: 550)
                .AddTitle("Số lượng người đăng ký trong năm nay")
                .AddSeries( 
                chartType: "Column", 
                xValue: new[] {"Thg1", "Thg2", "Thg3", "Thg4", "Thg5", "Thg6", "Thg7", "Thg8", "Thg9", "Thg10", "Thg11", "Thg12" }
                , yValues: new[] {y1, y2, y3, y4, y5, y6, y7, y8, y9, y10, y11, y12 })
                .Write("png");
            return null;
        }
        [Filter.AuthorizeAdmin]
        public ActionResult tkDayhoc()
        {
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var result = model.history_lessons.ToList();
            decimal y1 = model.history_lessons.Count(x => x.day.Value.Month == 1);
            decimal y2 = model.history_lessons.Count(x => x.day.Value.Month == 2);
            decimal y3 = model.history_lessons.Count(x => x.day.Value.Month == 3);
            decimal y4 = model.history_lessons.Count(x => x.day.Value.Month == 4);
            decimal y5 = model.history_lessons.Count(x => x.day.Value.Month == 5);
            decimal y6 = model.history_lessons.Count(x => x.day.Value.Month == 6);
            decimal y7 = model.history_lessons.Count(x => x.day.Value.Month == 7);
            decimal y8 = model.history_lessons.Count(x => x.day.Value.Month == 8);
            decimal y9 = model.history_lessons.Count(x => x.day.Value.Month == 9);
            decimal y10 = model.history_lessons.Count(x => x.day.Value.Month == 10);
            decimal y11 = model.history_lessons.Count(x => x.day.Value.Month == 11);
            decimal y12 = model.history_lessons.Count(x => x.day.Value.Month == 12);

            //for(int i = 0; i < 12; i++)
            //{
            //    int u = i + 1;
            //    xValue[i] = "Tháng " + u;
            //}

            new Chart(width: 800, height: 550)
                .AddTitle("Số lượng các buổi học đã dạy trong năm nay")
                .AddSeries(
                chartType: "Column",
                xValue: new[] { "Thg1", "Thg2", "Thg3", "Thg4", "Thg5", "Thg6", "Thg7", "Thg8", "Thg9", "Thg10", "Thg11", "Thg12" }
                , yValues: new[] { y1, y2, y3, y4, y5, y6, y7, y8, y9, y10, y11, y12 })
                .Write("png");
            return null;
        }
        [Filter.AuthorizeAdmin]
        public ActionResult tkDangkyhoc()
        {
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var result = model.sessions.ToList();
            decimal y1 = model.sessions.Count(x => x.dateCreate.Value.Month == 1);
            decimal y2 = model.sessions.Count(x => x.dateCreate.Value.Month == 2);
            decimal y3 = model.sessions.Count(x => x.dateCreate.Value.Month == 3);
            decimal y4 = model.sessions.Count(x => x.dateCreate.Value.Month == 4);
            decimal y5 = model.sessions.Count(x => x.dateCreate.Value.Month == 5);
            decimal y6 = model.sessions.Count(x => x.dateCreate.Value.Month == 6);
            decimal y7 = model.sessions.Count(x => x.dateCreate.Value.Month == 7);
            decimal y8 = model.sessions.Count(x => x.dateCreate.Value.Month == 8);
            decimal y9 = model.sessions.Count(x => x.dateCreate.Value.Month == 9);
            decimal y10 = model.sessions.Count(x => x.dateCreate.Value.Month == 10);
            decimal y11 = model.sessions.Count(x => x.dateCreate.Value.Month == 11);
            decimal y12 = model.sessions.Count(x => x.dateCreate.Value.Month == 12);

            //for(int i = 0; i < 12; i++)
            //{
            //    int u = i + 1;
            //    xValue[i] = "Tháng " + u;
            //}

            new Chart(width: 800, height: 550)
                .AddTitle("Số lượng đăng ký học dạy kèm trong năm nay")
                .AddSeries(
                chartType: "Column",
                xValue: new[] { "Thg1", "Thg2", "Thg3", "Thg4", "Thg5", "Thg6", "Thg7", "Thg8", "Thg9", "Thg10", "Thg11", "Thg12" }
                , yValues: new[] { y1, y2, y3, y4, y5, y6, y7, y8, y9, y10, y11, y12 })
                .Write("png");
            return null;
        }
        [Filter.AuthorizeAdmin]
        public ActionResult User()
        {
            return View();
        }



        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Duyetkhoahoc(int id)
        {
            int asd = id;
            var se = model.sessions.Find(id);
            se.status_admin = 1;
            model.SaveChanges();
            return RedirectToAction("Duyetkhoahoc");
        }

        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Duyetschedule(int id)
        {
            int asd = id;
            var se = model.schedules.Find(id);
            se.status = 1;
            model.SaveChanges();
            return RedirectToAction("Duyetschedule");
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Contact()
        {
            var contact = model.contact_admin.ToList();
            return View(contact);
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Blockuser()
        {

            return View();
        }

        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Duyettutor(int id)
        {
            int asd = id;
            var se = model.tutors.Find(id);
            se.status = 1;
            model.SaveChanges();
            return RedirectToAction("User", "Admin");
        }

        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Duyetparent(int id)
        {
            int asd = id;
            var se = model.parents.Find(id);
            se.status = 1;
            model.SaveChanges();
            return RedirectToAction("User", "Admin");
        }

        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Duyetstudent(int id)
        {
            int asd = id;
            var se = model.students.Find(id);
            se.status = 1;
            model.SaveChanges();
            return RedirectToAction("User","Admin");
        }

        //khoa
        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Khoatutor(int id)
        {
            int asd = id;
            var se = model.tutors.Find(id);
            if (se.status == 1)
            {
                se.status = 3;
                model.SaveChanges();
            }
            else if (se.status == 3)
            {
                se.status = 1;
                model.SaveChanges();
            }
            return RedirectToAction("Blockuser");
        }

        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Khoaparent(int id)
        {
            int asd = id;
            var se = model.parents.Find(id);
            if (se.status == 1)
            {
                se.status = 3;
                model.SaveChanges();
            }
            else if (se.status == 3)
            {
                se.status = 1;
                model.SaveChanges();
            }
            return RedirectToAction("Blockuser");
        }

        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Khoastudent(int id)
        {
            int asd = id;
            var se = model.students.Find(id);
            if (se.status == 1)
            {
                se.status = 3;
                model.SaveChanges();
            }
            else if (se.status == 3)
            {
                se.status = 1;
                model.SaveChanges();
            }
            return RedirectToAction("Blockuser");
        }
    }
}
