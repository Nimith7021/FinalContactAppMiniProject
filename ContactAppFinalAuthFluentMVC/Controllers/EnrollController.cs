using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ContactAppFinalAuthFluentMVC.Data;
using ContactAppFinalAuthFluentMVC.Models;
using ContactAppFinalAuthFluentMVC.ViewModels;
using NHibernate.Mapping;

namespace ContactAppFinalAuthFluentMVC.Controllers
{
    public class EnrollController : Controller
    {
        // GET: LogIn
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]

        public ActionResult LogIn(LoginVM loginVM)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var targetUser = session.Query<User>().FirstOrDefault(u => u.FName == loginVM.UserName);
                if (targetUser.IsActive == false)
                {
                    ModelState.AddModelError("", "User is Deactivated");
                    return View();
                }
                HttpCookie cookie = new HttpCookie("myCookie");
                cookie.Value = (targetUser.Id).ToString();
                cookie.Expires = DateTime.Now.AddDays(15);
                Response.Cookies.Add(cookie);
                if (targetUser != null && BCrypt.Net.BCrypt.Verify(loginVM.Password, targetUser.Password))
                {
                    
                    FormsAuthentication.SetAuthCookie(loginVM.UserName, true);
                    return RedirectToAction("Index", "User");
                }

                ModelState.AddModelError("", "Username/Password doesn't exist");
                return View();
            }
        }

        public ActionResult Register()
        {
            

            return View();

        }

        [HttpPost]

        public ActionResult Register(User user)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                if (user.IsAdmin == true)
                {
                    user.Role.User = user;
                    user.Role.RoleName = "Admin";
                }
                else
                {
                    user.Role.User = user;
                    user.Role.RoleName = "Staff";
                }
                using (var txn = session.BeginTransaction())
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    session.Save(user);
                    txn.Commit();
                    return RedirectToAction("LogIn");
                }
            }
        }


        public ActionResult LogOut()
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("LogIn");
            } 
        }
    }
}
