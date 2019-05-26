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
    public class ParentController : BaseController
    {
        eTUITOREntities db = new eTUITOREntities();
        // GET: Parent
        public ActionResult InfoOfParent()
        {
            if (Session["UserID"] != null)
            {
                var parent_id = int.Parse(Session["UserID"].ToString());
                var info = db.parents.FirstOrDefault(x => x.parent_id == parent_id);
                return View(info);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        public ActionResult listPr()
        {
            var listpr = db.parents.ToList();
            return View(listpr);
        }
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var pr = db.parents.FirstOrDefault(x => x.parent_id == id);
            return View(pr);
        }
        public ActionResult listRegistCourse()
        {
            string id = Session["username"].ToString();
            var prt = db.parents.FirstOrDefault(x => x.username == id);
            var listCourse = db.sessions.ToList().Where(x => x.status_tutor == 2 && x.student_id == prt.parent_id);
            return View(listCourse);
        }
        public ActionResult SessionOfChilds()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateChildAccount(student student, HttpPostedFileBase avatarSon)
        {

            if (ModelState.IsValid)
            {
                if (avatarSon == null)
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
                else if (avatarSon.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 3; //3 MB
                    string[] AllowedFileExtensions = new string[] { ".jpg", ".png" };

                    if (!AllowedFileExtensions.Contains(avatarSon.FileName.Substring(avatarSon.FileName.LastIndexOf('.'))))
                    {
                        setAlert("Vui lòng chọn file có đuôi : .JPG .PNG", "error");
                        return RedirectToAction("InfoOfParent");
                    }

                    else if (avatarSon.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                    }
                    else
                    {
                        //TO:DO
                        var fileName = Path.GetFileName(avatarSon.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
                        avatarSon.SaveAs(path);
                        ModelState.Clear();
                        student.parent_id = int.Parse(Session["UserID"].ToString());
                        student.dateCreate = DateTime.Now;
                        student.avatar = path;
                        db.students.Add(student);
                        db.SaveChanges();
                        return RedirectToAction("InfoOfParent", "Parent", new { id = Session["UserID"] });
                    }
                }
            }
            setAlert("Vui lòng tải ảnh đại diện cho con của bạn", "danger");
            return View("InfoOfParent");

        }
        //Update Avatar
        [Filter.Authorize]
        [HttpPost]
        public ActionResult changeAvatar(HttpPostedFileBase files)
        {
            if (files == null)
            {
                setAlert("Vui lòng chọn file !", "error");
                return RedirectToAction("InfoOfParent");
            }
            else if (files.ContentLength > 0)
            {
                int MaxContentLength = 1024 * 1024 * 4; //3 MB
                string[] AllowedFileExtensions = new string[] { ".jpg", ".png" };
                if (!AllowedFileExtensions.Contains(files.FileName.Substring(files.FileName.LastIndexOf('.'))))
                {
                    setAlert("Vui lòng chọn file có đuôi : .JPG .PNG", "error");
                    return RedirectToAction("InfoOfParent");
                }
                else if (files.ContentLength > MaxContentLength)
                {
                    setAlert("File bạn tải lên quá lớn, tối đa :" + 4 + "MB", "error");
                    return RedirectToAction("InfoOfParent");
                }
                else
                {
                    //todo
                    var fileName = Path.GetFileName(files.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img/avatar/parent"), fileName);
                    files.SaveAs(path);
                    string s = Session["UserID"].ToString();
                    int idUser = int.Parse(s);
                    //get parent
                    var parent = db.parents.SingleOrDefault(x => x.parent_id == idUser);
                    //xóa file ảnh cũ
                    var photoName = parent.avatar;
                    if (photoName != null)
                    {
                        var fullPath = Path.Combine(Server.MapPath("~/Content/img/avatar/parent"), photoName);

                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    parent.avatar = fileName;
                    db.SaveChanges();
                    setAlert("Tải ảnh đại diện thành công", "success");
                    ModelState.Clear();
                    return RedirectToAction("InfoOfParent");
                }
            }

            setAlert("Vui lòng chọn file", "error");
            return RedirectToAction("InfoOfParent");
        }
        //Edit info of Son
        public ActionResult EditSon(string idStudent, string fullname, string username, string email, string bod, HttpPostedFileBase files)
        {
            if (files == null)
            {
                setAlert("Vui lòng chọn file !", "error");
                return RedirectToAction("InfoOfParent", "Parent");
            }
            else if (files.ContentLength > 0)
            {
                int MaxContentLength = 1024 * 1024 * 3; //3 MB
                string[] AllowedFileExtensions = new string[] { ".jpg", ".png", ".pdf" };
                if (!AllowedFileExtensions.Contains(files.FileName.Substring(files.FileName.LastIndexOf('.'))))
                {
                    setAlert("Vui lòng chọn file có đuôi : .JPG .PNG", "error");
                    return RedirectToAction("InfoOfParent", "Parent");
                }
                else if (files.ContentLength > MaxContentLength)
                {
                    setAlert("File bạn tải lên quá lớn, tối đa :" + MaxContentLength + "MB", "error");
                    return RedirectToAction("InfoOfParent", "Parent");
                }
                else
                {
                    //TO:DO
                    var fileName = Path.GetFileName(files.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img/avatar/student"), fileName);
                    files.SaveAs(path);
                    //get student
                    int n = int.Parse(idStudent);
                    var student = db.students.SingleOrDefault(x => x.student_id == n);
                    //xóa file ảnh cũ
                    var photoName = student.avatar;
                    var fullPath = Path.Combine(Server.MapPath("~/Content/img/avatar/student"), photoName);
                    if (System.IO.File.Exists(fullPath))

                    {
                        System.IO.File.Delete(fullPath);
                    }
                    student.avatar = fileName;
                    student.birthday = DateTime.Parse(bod);
                    student.fullname = fullname;
                    student.email = email;
                    student.username = username;
                    db.SaveChanges();
                    setAlert("Tải ảnh đại diện thành công", "success");
                    ModelState.Clear();
                    return RedirectToAction("InfoOfParent", "Parent");
                }
            }

            setAlert("Vui lòng chọn file", "error");
            return RedirectToAction("InfoOfParent", "Parent");
        }
    }
}