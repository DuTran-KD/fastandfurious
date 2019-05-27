using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
using eTUTOR.Service;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mail;
using System.Web.UI;
using System.Net.Mail;
using System.Web.Security;
using eTUTOR.Filter;

namespace eTUTOR.Controllers
{

    public class UserController : BaseController
    {
        //===================================================
        eTUITOREntities model = new eTUITOREntities();
        CommonService commonService = new CommonService();
        //===================================================

        [HttpGet]
        public ActionResult Register()
        {
            var user = new tutor();
            return View(user);
        }
        [HttpPost]
        public ActionResult RegisterTuTor(tutor tutor/*, HttpPostedFileBase certificate, string password, string email*/)
        {
            string email = "";
            string password = "";
            var tutorEmail = model.tutors.FirstOrDefault(x => x.email == tutor.email || x.username == tutor.username);
            

            if (tutorEmail == null)
            {

                //add new tutor
                tutor.status = 2;
                //if (certificate != null && certificate.ContentLength > 0)
                //{
                //    tutor.certificate = certificate.FileName;
                //}
                tutor.specialized = tutor.specialized.ToUpper();


                tutor.password = commonService.hash(tutor.password);
                tutor.dateCreate = DateTime.Now;
                tutor.status_register = 1;
                model.tutors.Add(tutor);
                model.SaveChanges();

                //save certificate

                //create directory
                string AppPath = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = AppPath + String.Format("Content\\img\\certificates\\{0}", tutor.tutor_id);
                DirectoryInfo direc = Directory.CreateDirectory(filePath);

                //save certificate
                //if (certificate != null && certificate.ContentLength > 0)
                //{
                //    string fileName = Path.GetFileName(certificate.FileName);
                //    string path = String.Format("{0}\\{1}", filePath, fileName);
                //    certificate.SaveAs(path);
                //}
                return RedirectToAction("ConfirmEmail", "User");

            }
            else
            {
                ViewBag.Er = "Tài khoản đã tồn tại !";
                return View("Register");
            }

        }
        public ActionResult RegisterParent(parent parent/*, string password*/)
        {
            string password = "";
            var parentEmail = model.parents.FirstOrDefault(x => x.email == parent.email || x.username == parent.username);
            
            if (parentEmail == null)
            {

                parent.status = 2;
                parent.password = commonService.hash(parent.password);
                parent.dateRegist = DateTime.Now;
                model.parents.Add(parent);
                model.SaveChanges();
                return RedirectToAction("ConfirmEmail", "User");
            }
            else
            {
                ViewBag.Er = "Tài khoản đã tồn tại !";
                return View("Register");
            }
        }

        public ActionResult RegisterStudent(student student/*, string password*/)
        {
            string password = "";
            var studentEmail = model.students.FirstOrDefault(x => x.email == student.email || x.username == student.username);
            
            if (studentEmail == null)
            {
                student.status = 2;
                student.password = commonService.hash(student.password);
                student.dateCreate = DateTime.Now;
                model.students.Add(student);
                model.SaveChanges();
                return RedirectToAction("ConfirmEmail", "User");
            }
            else
            {
                ViewBag.Er = "Tài khoản đã tồn tại !";
                return View("Register");
            }

        }
        
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public ActionResult CheckForgotPW(string username, string email)
        {
            var student = model.students.FirstOrDefault(x => x.username == username);
            var parent = model.parents.FirstOrDefault(x => x.username == username);
            var tutorr = model.tutors.FirstOrDefault(x => x.username == username);
            if (student != null)
            {
                if (student.email != email)
                {
                    ViewBag.Err = "Email sai rồi !";
                    return View("ForgotPassword");
                }
                string newPW = CreateLostPassword(10);
                CapNhatMatKhau(student.username, newPW, "std");
                guiMail(student.username, newPW, email);

                setAlert("Mật khẩu đã được thay đổi !", "success");
                return View("Login");

            }
            if (parent != null)
            {
                if (parent.email != email)
                {
                    ViewBag.Err = "Email sai rồi !";
                    return View("ForgotPassword");
                }
                string newPW = CreateLostPassword(10);
                CapNhatMatKhau(parent.username, newPW, "prt");
                guiMail(parent.username, newPW, email);
                ViewBag.sc = "Mật khẩu đã được thay đổi !";
                return View("Login");
            }
            if (tutorr != null)
            {
                if (tutorr.email != email)
                {
                    ViewBag.Err = "Email sai rồi !";
                    return View("ForgotPassword");
                }
                string newPW = CreateLostPassword(10);
                CapNhatMatKhau(tutorr.username, newPW, "tutorr");
                guiMail(tutorr.username, newPW, email);
                ViewBag.sc = "Mật khẩu đã được thay đổi !";
                return View("Login");
            }
            else
            {
                ViewBag.Err = "Tên đăng nhập sai rồi !";
                return View("ForgotPassword");
            }


        }
        public ActionResult ConfirmEmail()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                TempData["returnUrl"] = returnUrl;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password, string returnUrl)
        {
            var tutor = model.tutors.FirstOrDefault(x => x.email == email || x.username == email);
            var student = model.students.FirstOrDefault(x => x.email == email || x.username == email);
            var parent = model.parents.FirstOrDefault(x => x.email == email || x.username == email);

            password = commonService.hash(password);
            if (tutor != null)
            {
                if (tutor.password.Equals(password))
                {
                    //check status of account
                    if (tutor.status == 1)
                    {
                        var jwt = new JWT();
                        JSONObject dataUser = jwt.DataJSON(tutor);
                        string token = jwt.TokenStream(dataUser);
                        Session["FullName"] = tutor.fullname;
                        Session["UserID"] = tutor.tutor_id;
                        Session["username"] = tutor.username;
                        Session["Role"] = "tutor";
                        Session.Timeout = 180;
                        if (TempData["returnUrl"] != null && !string.IsNullOrWhiteSpace(TempData["returnUrl"].ToString()))
                        {
                            return Json(new { url= TempData["returnUrl"].ToString(), TokenContext = token }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { url = "Tutor/InfoOfTutor", TokenContext = token }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    
                    if (tutor.status == 3)
                    {
                        ViewBag.msg1 = "Tài khoản của bạn đã bị khóa , vui lòng liên hệ ban quản trị hệ thống";
                        return Json(new { error = "Block", message = ViewBag.msg1, status = 3 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ViewBag.msg = "Tài khoản của bạn chưa được kích hoạt";
                        return Json(new { error = "No Access", message = ViewBag.msg, status = 0 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ViewBag.msg = "Mật khẩu sai rồi !";
                    return Json(new { error = "No Access", message = ViewBag.msg, status = 0 }, JsonRequestBehavior.AllowGet);
                }

            }
            if (student != null)
            {
                if (student.password.Equals(password))
                {
                    if (student.status == 1)
                    {
                        var jwt = new JWT();
                        JSONObject dataUser = jwt.DataJSON(student);
                        string token = jwt.TokenStream(dataUser);
                        Session["FullName"] = student.fullname;
                        Session["username"] = student.username;
                        Session["UserID"] = student.student_id;
                        Session["username"] = student.username;
                        Session["Role"] = "student";
                        Session.Timeout = 180;
                        if (TempData["returnUrl"] != null && !string.IsNullOrWhiteSpace(TempData["returnUrl"].ToString()))
                        {
                            return Json(new { url = TempData["returnUrl"].ToString(), TokenContext = token }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { url = "Student/InfoOfStudent", TokenContext = token }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (student.status == 2)
                    {
                        ViewBag.msg = "Tài khoản của bạn chưa được kích hoạt";
                        return Json(new { error = "No Access", message = ViewBag.msg, status = 2 }, JsonRequestBehavior.AllowGet);
                    }
                    if (student.status == 3)
                    {
                        ViewBag.msg1 = "Tài khoản của bạn đã bị khóa , vui lòng liên hệ ban quản trị hệ thống";
                        return Json(new { error = "No Access", message = ViewBag.msg1, status = 3 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ViewBag.msg = "Mật khẩu sai rồi !";
                    return Json(new { error = "No Access", message = ViewBag.msg, status = 0 }, JsonRequestBehavior.AllowGet);
                }

            }

            if (parent != null)
            {
                if (parent.password.Equals(password))
                {
                    if (parent.status == 1)
                    {
                        var jwt = new JWT();
                        JSONObject dataUser = jwt.DataJSON(parent);
                        string token = jwt.TokenStream(dataUser);
                        Session["FullName"] = parent.fullname;
                        Session["username"] = parent.username;
                        Session["UserID"] = parent.parent_id;
                        Session["username"] = parent.username;
                        Session["Role"] = "parent";
                        Session.Timeout = 180;
                        if (TempData["returnUrl"] != null && !string.IsNullOrWhiteSpace(TempData["returnUrl"].ToString()))
                        {
                            return Json(new { url = TempData["returnUrl"].ToString(), TokenContext = token }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { url = "Parent/InfoOfParent", TokenContext = token }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (parent.status == 2)
                    {
                        ViewBag.msg = "Tài khoản của bạn chưa được kích hoạt";
                        return Json(new { error = "No Access", message = ViewBag.msg, status = 2 }, JsonRequestBehavior.AllowGet);
                    }
                    if (parent.status == 3)
                    {
                        ViewBag.msg1 = "Tài khoản của bạn đã bị khóa , vui lòng liên hệ ban quản trị hệ thống";
                        return Json(new { error = "No Access", message = ViewBag.msg1, status = 3 }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    ViewBag.msg = "Mật khẩu sai rồi !";
                    return Json(new { error = "No Access", message = ViewBag.msg, status = 0 }, JsonRequestBehavior.AllowGet);
                }

            }
            ViewBag.msg = "username hoặc email không tồn tại";
            return Json(new { error = "No Access", message = ViewBag.msg, status = 0 }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        //tạo pass mới ngẫu nhiên
        public string CreateLostPassword(int PasswordLength)
        {
            string _allowedChars = "abcdefghijk0123456789mnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random(); char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        private void CapNhatMatKhau(string TenDangNhap, string MatKhau, string typeUser)
        {
            if (typeUser == "std")
            {
                var stdd = model.students.FirstOrDefault(x => x.username == TenDangNhap);
                stdd.password = commonService.hash(MatKhau);
                model.SaveChanges();

            }
            else if (typeUser == "tutorr")
            {
                var tt = model.tutors.FirstOrDefault(x => x.username == TenDangNhap);
                tt.password = commonService.hash(MatKhau);
                model.SaveChanges();
            }
            else
            {
                var pr = model.parents.FirstOrDefault(x => x.username == TenDangNhap);
                pr.password = commonService.hash(MatKhau);
                model.SaveChanges();
            }
        }
        //nội dung gửi maill
        private string NoiDungMail(string TenDangNhap, string PW)
        {
            string NoiDung = "";
            NoiDung = "Đây là Mail gửi đến từ website của ETUTOR. ";
            NoiDung += "Mật khẩu mới của bạn là:  " + PW;
            NoiDung += "  . Sau khi đăng nhập bạn nhớ đổi lại mật khẩu để tiện cho việc đăng nhập lần tiếp theo";
            NoiDung += ". Vui lòng không trả lời Mail này!";
            return NoiDung;
        }
        public void guiMail(string TenDangNhap, string PW, string mail)
        {
            /*
            MailMessage objEmail = new MailMessage
            {
                To = mail,
                From = "trancongdu1997@gmail.com",
                Subject = "Thông tin về mật khẩu của bạn",
                BodyEncoding = Encoding.UTF8,
                Body = NoiDungMail(TenDangNhap, PW),
                Priority = MailPriority.High,
                BodyFormat = MailFormat.Html
            };

            try
            {
                SmtpMail.Send(objEmail);
            }
            catch (Exception exc)
            {
                Response.Write("Send failure: " + exc.ToString());
            }
            */
            using (System.Net.Mail.MailMessage emailMessage = new System.Net.Mail.MailMessage())
            {
                emailMessage.From = new MailAddress("td159855@gmail.com");
                emailMessage.To.Add(new MailAddress(mail));
                emailMessage.Subject = "eTUTOR - Lay lai mat khau";
                emailMessage.Body = NoiDungMail(TenDangNhap, PW);
                emailMessage.Priority = System.Net.Mail.MailPriority.Normal;
                using (SmtpClient MailClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    MailClient.EnableSsl = true;
                    MailClient.Credentials = new System.Net.NetworkCredential("td159855@gmail.com", "215437331");
                    MailClient.Send(emailMessage);
                }
            }
        }
        public ActionResult ChangePW()
        {
            return View();
        }
        //Đổi mk
        [HttpPost]
        public ActionResult ChangePW(string currentPW, string newPW, string cfNewPW)
        {
            if (currentPW == string.Empty || newPW == string.Empty || cfNewPW == string.Empty)
            {

                ViewBag.iff = "Vui lòng điền đầy đủ thông tin";
                setAlert(ViewBag.iff, "warning");
                return View("ChangePW");

            }
            else if (newPW == cfNewPW)
            {
                string s = Session["username"].ToString();
                var tutor = model.tutors.FirstOrDefault(x => x.username == s);
                var student = model.students.FirstOrDefault(x => x.username == s);
                var parent = model.parents.FirstOrDefault(x => x.username == s);
                if (tutor != null)
                {
                    string passCur = commonService.hash(currentPW);
                    if (tutor.password != passCur)
                    {
                        ViewBag.iff = "Mật khẩu hiện tại không đúng";
                        setAlert(ViewBag.iff, "warning");
                        return View("ChangePW");
                    }
                    else
                    {
                        newPW = commonService.hash(newPW);
                        tutor.password = newPW;
                        model.SaveChanges();
                        setAlert("Đổi mật khẩu thành công", "success");
                        return RedirectToAction("InfoOfTutor", "Tutor");
                    }
                }
                if (student != null)
                {

                    string passCur = commonService.hash(currentPW);
                    if (student.password != passCur)
                    {
                        ViewBag.iff = "Mật khẩu hiện tại không đúng";
                        setAlert(ViewBag.iff, "warning");
                        return View("ChangePW");
                    }
                    else
                    {
                        newPW = commonService.hash(newPW);
                        student.password = newPW;
                        model.SaveChanges();
                        setAlert("Đổi mật khẩu thành công", "success");
                        return RedirectToAction("InfoOfStudent", "Student");
                    }

                }

                if (parent != null)
                {

                    string passCur = commonService.hash(currentPW);
                    if (parent.password != passCur)
                    {
                        ViewBag.iff = "Mật khẩu hiện tại không đúng";

                        setAlert(ViewBag.iff, "warning");
                        return View("ChangePW");
                    }
                    else
                    {
                        newPW = commonService.hash(newPW);
                        parent.password = newPW;
                        model.SaveChanges();
                        setAlert("Đổi mật khẩu thành công", "success");
                        return View("InfoOfParent", "Parent");
                    }

                }
                return View("ChangePW");
            }
            else
            {
                ViewBag.iff = "Xác nhận mật khẩu mới không trùng khớp";
                setAlert(ViewBag.iff, "warning");
                return View("ChangePW");
            }

        }
        [HttpPost]
        public ActionResult danhgiatutor(string ttID, string message)
        {
            string s = Session["UserID"].ToString();
            comment newcm = new comment();
            newcm.content = message;
            newcm.tutor_id = int.Parse(ttID);
            newcm.student_id = int.Parse(s);
            newcm.dateTimeCmt = DateTime.Now;
            model.comments.Add(newcm);
            model.SaveChanges();
            setAlert("Cảm ơn bạn đã đóng góp ý kiến", "success");
            return RedirectToAction("ViewDetailTutor", "Tutor", new { id = int.Parse(ttID) });
        }

    }
}

