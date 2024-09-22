using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using ContactAppFinalAuthFluentMVC.Data;
using ContactAppFinalAuthFluentMVC.DTO;
using ContactAppFinalAuthFluentMVC.Models;
using FluentNHibernate.Conventions.Inspections;
using Microsoft.AspNetCore.Mvc;

namespace ContactAppFinalAuthFluentMVC.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetContactById()
        {



            HttpCookie Id = Request.Cookies["myCookie"];
            using (var session = NHibernateHelper.CreateSession())
            {
                var contacts = session.Query<Contact>().Where(c => c.User.Id == Guid.Parse(Id.Value)).ToList();
                var target = contacts.Select(c => Mapper.ToDTo(c)).ToList();
                if (contacts.Count == 0)
                {
                    return HttpNotFound();
                }

                return Json(target, JsonRequestBehavior.AllowGet);
            }
        }


        
        public ActionResult GetContact(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var target = session.Query<Contact>().FirstOrDefault(c=>c.Id==id);
                if (!target.IsActive)
                {
                    return Json(new { success = false });
                }
                var convertedTarget = Mapper.ToDTo(target);
                return Json(convertedTarget, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetContacts(Guid userId)
        {

            using (var session = NHibernateHelper.CreateSession())
            {
                var contacts = session.Query<Contact>().Where(c => c.User.Id == userId).ToList();

                return View(contacts);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(Contact contact)
        {
            HttpCookie Id = Request.Cookies["myCookie"];
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    contact.User.Id = Guid.Parse(Id.Value);
                    session.Save(contact);
                    txn.Commit();
                    return Json(new { success = true });
                }
            }

        }

        public ActionResult Edit(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var contact = session.Get<Contact>(id);
                return View(contact);
            }
        }

        [HttpPost]
        public ActionResult Edit(Contact contact)
        {
            HttpCookie Id = Request.Cookies["myCookie"];
            //Guid id = (Guid)Session["UserId"];
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    contact.User.Id = Guid.Parse(Id.Value);
                    session.Update(contact);
                    txn.Commit();
                    return Json(new { success = true });
                }
            }
        }

        [HttpPost]
        public JsonResult EditContactStatus(int userId, bool isActive)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var targetContact = session.Get<Contact>(userId);
                using (var txn = session.BeginTransaction())
                {
                    targetContact.IsActive = isActive;
                    session.Update(targetContact);
                    txn.Commit();
                    return Json(new { success = true });
                }
            }
        }
    }
}