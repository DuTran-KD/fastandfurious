using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;

namespace eTUTOR.Controllers
{
    [Filter.Authorize]
    public class StudentController : BaseController
    {
        eTUITOREntities db = new eTUITOREntities();
        // GETA : Student
        [Filter.Authorize]
        public ActionResult InfoOfStudent()
        {
            if(Session["UserID"] != null) {
                var student_id = int.Parse(Session["UserID"].ToString());
                var info = db.students.FirstOrDefault(x => x.student_id == student_id);
                return View(info);
            } else
            {
                return RedirectToAction("Login", "User");
            }
        }
        public ActionResult listRegistCourse()
        {
            string id = Session["username"].ToString();
            var prt = db.students.FirstOrDefault(x => x.username == id);
            var listCourse = db.sessions.ToList().Where(x => x.status_tutor == 2 && x.student_id == prt.student_id);
            return View(listCourse);
        }
        public ActionResult deleteRegister(int id)
        {
            var ss = db.sessions.FirstOrDefault(x => x.session_id == id);
            db.sessions.Remove(ss);
            db.SaveChanges();
            return RedirectToAction("listRegistCourse");
        }

        public ActionResult SessionOfStudent(string p)
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
                ViewData["url"] = Request.Url.AbsoluteUri.ToString();
            }
            else
            {
                ViewData["url"] = Request.Url.AbsoluteUri.ToString();
            }

            ViewData["GroupName"] = p;
            ViewBag.GroupName = p;

            return View();
        }
        //Update Avatar
        [Filter.Authorize]
        [HttpPost]
        public ActionResult changeAvatar(HttpPostedFileBase files)
        {


            if (files == null)
            {

                setAlert("Vui lòng chọn file !", "error");
                return RedirectToAction("InfoOfStudent");
            }
            else if (files.ContentLength > 0)
            {
                int MaxContentLength = 1024 * 1024 * 3; //3 MB
                string[] AllowedFileExtensions = new string[] { ".jpg", ".png"};

                if (!AllowedFileExtensions.Contains(files.FileName.Substring(files.FileName.LastIndexOf('.'))))
                {
                    setAlert("Vui lòng chọn file có đuôi : .JPG .PNG", "error");
                    return RedirectToAction("InfoOfStudent");
                }

                else if (files.ContentLength > MaxContentLength)
                {
                    setAlert("File bạn tải lên quá lớn, tối đa :" + MaxContentLength +"MB", "error");
                    return RedirectToAction("InfoOfStudent");
                }
                else
                {
                    //TO:DO

                    var fileName = Path.GetFileName(files.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img/avatar/student"), fileName);
                    files.SaveAs(path);
                    string s = Session["UserID"].ToString();
                    int idUser = int.Parse(s);
                    //get parent
                    var student = db.students.SingleOrDefault(x => x.student_id == idUser);
                    //xóa file ảnh cũ
                    var photoName = student.avatar;
                    var fullPath = Path.Combine(Server.MapPath("~/Content/img/avatar/student"), photoName);

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    student.avatar = fileName;
                    db.SaveChanges();
                    setAlert("Tải ảnh đại diện thành công", "success");
                    ModelState.Clear();
                    return RedirectToAction("InfoOfStudent");
                }
            }

            setAlert("Vui lòng chọn file", "error");
            return RedirectToAction("InfoOfStudent");
        }
    }
}