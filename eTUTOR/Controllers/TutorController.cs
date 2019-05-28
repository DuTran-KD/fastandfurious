using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
using eTUTOR.Service;
using Newtonsoft.Json.Linq;


namespace eTUTOR.Controllers
{



    public class TutorController : BaseController
    {
        eTUITOREntities db = new eTUITOREntities();
        CommonService commonService = new CommonService();
        // GET: Tutor
        [AllowAnonymous]
        public ActionResult ListOfTutors()
        {

            var listTT = db.tutors.ToList().Where(x => x.status_register == 1 && x.status == 1);
            return View(listTT);
        }

        [HttpGet]
        public ActionResult ViewDetailTutor(int? id)
        {
            if (Session["Role"] != null)
            {
                ViewBag.typeUser = Session["Role"].ToString();
            }
            var tatu = db.tutors.FirstOrDefault(x => x.tutor_id == id);
            if (tatu == null)
            {
                return View();
            }
            else
            {
                return View(tatu);
            }
        }
        [Filter.Authorize]
        public ActionResult InfoOfTutor()
        {
            if (Session["UserID"] != null)
            {
                var tutor_id = int.Parse(Session["UserID"].ToString());
                var info = db.tutors.FirstOrDefault(x => x.tutor_id == tutor_id);
                List<session> sessionList = db.sessions.Where(x => x.tutor_id == tutor_id && x.status_tutor == 2 && x.status_admin == 2).ToList();
                ViewData["sessionlist"] = sessionList;
                List<schedule> scheduleList = db.schedules.Where(x => x.tutor_id == tutor_id).ToList();
                ViewData["scheduleList"] = scheduleList;
                return View(info);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        [Filter.Authorize]
        public ActionResult checkSession(int iddd, string types)
        {
            //kiểm tra khi returnUrl tránh bị lỗi session ở action RegisterTutor và RegisterTutorParent
            if (types == "student" || types == "tutor")
            {

                return RedirectToAction("RegisterTutor", new { id = iddd, type = types });
            }
            else
            {

                return RedirectToAction("RegisterTutorParent", new { idd = iddd });
            }
        }
        [Filter.Authorize]
        [HttpGet]
        public ActionResult RegisterTutor(int? id, string type)
        {

            if (Session["Role"].ToString() == "parent")
            {
                return RedirectToAction("RegisterTutorParent", new { idd = id });

            }
            var tatu = db.tutors.FirstOrDefault(x => x.tutor_id == id);
            if (tatu == null)
            {
                setAlert("Vui lòng chọn Tutor", "warning");
                RedirectToAction("ListOfTutors", "Tutor");
            }
            else if (type == "student" || type == "tutor")
            {
                return View(tatu);
            }

            return View(tatu);
        }
        [Filter.Authorize]
        [HttpGet]
        public ActionResult RegisterTutorParent(int? idd)
        {
            if (Session["Role"].ToString() != "parent")
            {
                return RedirectToAction("RegisterTutor", new { id = idd });

            }
            var tatu = db.tutors.FirstOrDefault(x => x.tutor_id == idd);
            if (tatu == null)
            {
                setAlert("Vui lòng chọn Tutor", "warning");
                RedirectToAction("ListOfTutors", "Tutor");
            }

            return View(tatu);

        }
        [Filter.Authorize]
        [HttpPost]
        public ActionResult ConfirmScheduleTutor(int idgiasu, int idmonhoc, int[] idschedule, int soBuoi)
        {
            string idst = Session["UserID"].ToString();
            int idStudent = int.Parse(idst);
            string id = Session["username"].ToString();
            var std = db.students.FirstOrDefault(x => x.username == id);
            if (std != null)
            {
                foreach (var item in idschedule)
                {
                    var schh = db.schedules.FirstOrDefault(x => x.schedule_id == item);
                    session newss = new session();
                    newss.day_otw = schh.day_otw;
                    newss.start_time = schh.start_time;
                    newss.end_time = schh.end_time;
                    newss.@class = std.@class.ToString();
                    newss.student_id = idStudent;
                    newss.tutor_id = idgiasu;
                    newss.total_sessions = soBuoi;
                    newss.subject_id = idmonhoc;
                    newss.status_admin = 2;
                    newss.status_tutor = 2;
                    newss.status_id = 2;
                    newss.schedule_id = schh.schedule_id;
                    newss.dateCreate = DateTime.Now;
                    db.sessions.Add(newss);
                    db.SaveChanges();
                }

                return RedirectToAction("listRegistCourse", "Student");
            }
            return RedirectToAction("Login", "User");

        }
        [Filter.Authorize]
        [HttpPost]
        public ActionResult ConfirmScheduleTutorParent(int idgiasu, int idmonhoc, int[] idschedule, int idSon, int soBuoi)
        {

            int idStudent = idSon;
            string id = Session["username"].ToString();
            var std = db.students.FirstOrDefault(x => x.student_id == idStudent);
            if (std != null)
            {
                foreach (var item in idschedule)
                {
                    var schh = db.schedules.FirstOrDefault(x => x.schedule_id == item);
                    session newss = new session();
                    newss.day_otw = schh.day_otw;
                    newss.start_time = schh.start_time;
                    newss.end_time = schh.end_time;
                    newss.@class = std.@class.ToString();
                    newss.student_id = idStudent;
                    newss.tutor_id = idgiasu;
                    newss.total_sessions = soBuoi;
                    newss.subject_id = idmonhoc;
                    newss.status_admin = 2;
                    newss.status_tutor = 2;
                    newss.status_id = 2;
                    newss.schedule_id = schh.schedule_id;
                    newss.dateCreate = DateTime.Now;
                    db.sessions.Add(newss);
                    db.SaveChanges();
                }

                return RedirectToAction("InfoOfParent", "Parent");

            }
            return RedirectToAction("Login", "User");

        }
        [Filter.Authorize]
        public ActionResult SessionOfTutor(string p)
        {
            ViewData["IsNewGroup"] = false;
            if (string.IsNullOrWhiteSpace(p))
            {
                //Guid g = Guid.NewGuid();
                //p = Convert.ToBase64String(g.ToByteArray());
                //p = p.Replace("=", "");
                //p = p.Replace("+", "");
                p = "demo";
                ViewData["IsNewGroup"] = true;
                ViewData["url"] = "http://localhost:52781/Student/SessionOfStudent?p=" + p;
            }
            else
            {
                ViewData["url"] = "http://localhost:52781/Student/SessionOfStudent";
            }

            ViewData["GroupName"] = p;
            ViewBag.GroupName = p;

            return View();
        }
        [Filter.Authorize]
        public ActionResult CreateSchedule(schedule schedule)
        {
            schedule.status = 2;
            schedule.dateCreate = DateTime.Now;
            schedule.tutor_id = int.Parse(Session["UserID"].ToString());
            db.schedules.Add(schedule);
            db.SaveChanges();
            return RedirectToAction("InfoOfTutor", "Tutor", new { id = Session["UserID"] });
        }

        [Filter.Authorize]

        public ActionResult DeleteSchedule(int id)
        {
            schedule sch = db.schedules.Single(x => x.schedule_id == id);
            db.schedules.Remove(sch);
            db.SaveChanges();
            return RedirectToAction("InfoOfTutor", "Tutor", new { id = Session["UserID"] });
        }

        public ActionResult SearchTutor(string search)
        {
            string searchM = search.ToUpper();
            var tutor = db.tutors.ToList().Where(x => x.status == 1 && (x.fullname.ToUpper().Contains(searchM) || x.specialized.ToUpper().Contains(searchM)));
            return View(tutor);
        }

        [Filter.Authorize]
        [HttpPost]
        public ActionResult Duyetkhoahoc(int id)
        {
            var se = db.sessions.Find(id);
            se.status_tutor = 1;
            db.SaveChanges();
            return RedirectToAction("Duyetkhoahoc");
        }

        [HttpPost]
        public ActionResult Contact(string name, string phone, string email, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderemail = new MailAddress("ppcrentalteam04@gmail.com", "tutor"); // mail tutor 
                    var receiveremail = new MailAddress("hoak21t@gmail.com", "Cong ty"); //mail cong ty

                    var password = "K21t1team04";// mật khẩu địa chỉ mail 
                    var sub = subject;
                    var body = "Tên: " + name + "Số điện thoại: " + phone + " Email: " + email + " Tiêu đề: " + subject + " Nội dung: " + message;
                    // nội dung tin nhắn


                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderemail.Address, password)


                    };

                    using (var mess = new MailMessage(senderemail, receiveremail)
                    {
                        Subject = subject,
                        Body = body
                    }
                    )
                    {
                        smtp.Send(mess);
                    }
                    return RedirectToAction("Confirm", "Tutor");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "There are some problem in sending email";
            }
            return View();
        }

        public ActionResult Confirm()
        {
            return View();
        }
        [Filter.Authorize]
        public ActionResult registScheduleManager(int types)
        {
            string s = types.ToString();
            int n = int.Parse(s);
            var tutor = db.tutors.FirstOrDefault(x => x.tutor_id == n);
            if (tutor.status_register == 1)
            {
                tutor.status_register = 2;
                db.SaveChanges();
            }
            else if (tutor.status_register == 2)
            {
                tutor.status_register = 1;
                db.SaveChanges();
            }
            setAlert("Bạn đã thay đổi thành công", "success");
            return RedirectToAction("InfoOfTutor");
        }

        //Update Avatar
        [Filter.Authorize]
        [HttpPost]
        public ActionResult changeAvatar(HttpPostedFileBase files)
        {


            if (files == null)
            {

                setAlert("Vui lòng chọn file !", "error");
                return RedirectToAction("InfoOfTutor");
            }
            else if (files.ContentLength > 0)
            {
                int MaxContentLength = 1024 * 1024 * 4; //3 MB
                string[] AllowedFileExtensions = new string[] { ".jpg", ".png", ".pdf" };

                if (!AllowedFileExtensions.Contains(files.FileName.Substring(files.FileName.LastIndexOf('.'))))
                {

                    setAlert("Vui lòng chọn file có đuôi : .JPG .PNG", "error");
                    return RedirectToAction("InfoOfTutor");
                }

                else if (files.ContentLength > MaxContentLength)
                {
                    setAlert("File bạn tải lên quá lớn, tối đa :" + 4 + "MB", "error");
                    return RedirectToAction("InfoOfTutor");
                }
                else
                {
                    //TO:DO

                    var fileName = Path.GetFileName(files.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img/avatar/tutor"), fileName);
                    files.SaveAs(path);
                    string s = Session["UserID"].ToString();
                    int idUser = int.Parse(s);
                    //get tutor
                    var tutor = db.tutors.SingleOrDefault(x => x.tutor_id == idUser);
                    //xóa file ảnh cũ
                    var photoName = tutor.avatar;
                    var fullPath = Path.Combine(Server.MapPath("~/Content/img/avatar/tutor"), photoName);

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    tutor.avatar = fileName;
                    db.SaveChanges();
                    setAlert("Tải ảnh đại diện thành công", "success");
                    ModelState.Clear();
                    return RedirectToAction("InfoOfTutor");
                }
            }

            setAlert("Vui lòng chọn file", "error");
            return RedirectToAction("InfoOfTutor");
        }

        /**Môn học của tutor
        * Thêm môn học
        * Xóa môn học
       **/
        [HttpPost]
        public ActionResult addSubject(string nameSubject, int idTutor)
        {
            string nameSub = nameSubject.Trim();
            try
            {
                if(nameSub != null) { 
                var subnew = new subject();
                subnew.subject_name = nameSubject;
                subnew.tutor_id = idTutor;
                db.subjects.Add(subnew);
                db.SaveChanges();
                setAlert("Thêm môn học thành công", "success");
                return RedirectToAction("InfoOfTutor");
                }
                else
                {
                    setAlert("Môn học không được để trống", "error");
                    return RedirectToAction("InfoOfTutor");
                }
            }
            catch
            {
                setAlert("Môn học mới thêm đã bị trùng !", "error");
                return View("InfoOfTutor");
            }


        }
        public ActionResult deleteSubject(int idSubject)
        {

            var subj = db.subjects.Single(x => x.subject_id == idSubject);
            var checkCourse = db.sessions.FirstOrDefault(x => x.subject_id == idSubject);
            if (subj != null)
            {
                if (checkCourse == null)
                {
                    try
                    {
                        db.subjects.Remove(subj);
                        db.SaveChanges();

                        setAlert("Xóa thành công", "success");
                        return RedirectToAction("InfoOfTutor");
                    }

                    catch
                    {
                        setAlert("Môn học này đang mở dạy kèm không xóa được", "error");
                        return View("InfoOfTutor");
                    }
                }
                else
                {

                    setAlert("Môn học này đang mở dạy kèm không xóa được", "error");
                    return View("InfoOfTutor");
                }

            }
            else
            {
                setAlert("Không tìm thấy môn học", "error");
                return View("InfoOfTutor");
            }

        }
    }
}