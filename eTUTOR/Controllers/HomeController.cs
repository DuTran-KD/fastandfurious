using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;

namespace eTUTOR.Controllers
{
    public class HomeController : Controller
    {
        eTUITOREntities model = new eTUITOREntities();
        public ActionResult Index()
        {
            var listTT = model.tutors.ToList().Where(x => x.status_register == 1).Take(6);
            return View(listTT);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            var contact_admin = new contact_admin();
            return View(contact_admin);
        }

        [HttpPost]
        public ActionResult Contact(contact_admin contact_admin, string fullname, string phone, string email, string title, string content)
        {
            contact_admin.fullname = fullname;
            contact_admin.phone = phone;
            contact_admin.email = email;
            contact_admin.title = title;
            contact_admin.content = content;
            model.contact_admin.Add(contact_admin);
            model.SaveChanges();
            return RedirectToAction("Confirm", "Home");
        }

        public ActionResult Confirm()
        {
            return View();
        }
    }
}