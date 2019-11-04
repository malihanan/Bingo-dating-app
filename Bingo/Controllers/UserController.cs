using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bingo.Models;

namespace Bingo.Controllers
{
    public class UserController : Controller
    {
        private BingoDbContext ctx = new BingoDbContext();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            return View(ctx.Users.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            ctx.Users.Add(user);
            ctx.SaveChanges();
            return View("List");
        }
        public ActionResult Edit()
        {
            object obj = Session["UserId"];
            int uId = Int32.Parse(obj.ToString());
            if (obj != null)
            {
                User user = ctx.Users.Find(uId);
                return View(user);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file1)
        {
            object obj = Session["UserId"];
            int uId = Int32.Parse(obj.ToString());
            if (obj != null)
            {
                var userToUpdate = ctx.Users.Find(uId);
                if (TryUpdateModel(userToUpdate, "", new string[] { "UserName", "FirstName", "LastName", "ProfilePicture", "Bio", "Likes", "Dislikes" }))
                {
                    try
                    {
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            userToUpdate.ProfilePicture = new byte[file1.ContentLength];
                            file1.InputStream.Read(userToUpdate.ProfilePicture, 0, file1.ContentLength);
                        }
                        ctx.SaveChanges();

                        return RedirectToAction("List");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                        throw e;
                    }
                }
            }
            return View(User);/*
            if (ModelState.IsValid)
            {
                if (file1 != null && file1.ContentLength > 0)
                {
                    user.ProfilePicture = new byte[file1.ContentLength];
                    file1.InputStream.Read(user.ProfilePicture, 0, file1.ContentLength);
                }
                ctx.Entry(user).State = EntityState.Modified;
                ctx.SaveChanges();
                return RedirectToAction("List");
            }
            return View(user);*/
        }
        public ActionResult AddProfilePicture()
        {
            object obj = Session["UserId"];
            if (obj != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult AddProfilePicture(HttpPostedFileBase file1)
        {
            if (file1 != null && file1.ContentLength > 0)
            {
                int uId = Int32.Parse(Session["UserId"].ToString());
                User user = ctx.Users.SingleOrDefault(p => p.UserId == uId);
                user.ProfilePicture = new byte[file1.ContentLength];
                file1.InputStream.Read(user.ProfilePicture, 0, file1.ContentLength);
                ctx.Entry(user).State = EntityState.Modified;
                ctx.SaveChanges();
            }
            return View("LoggedIn");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            var get_user = ctx.Users.SingleOrDefault(p => p.UserName == user.UserName
            && p.Password == user.Password);
            if (get_user != null)
            {
                Session["UserId"] = get_user.UserId.ToString();
                Session["UserName"] = get_user.UserName.ToString();
                return RedirectToAction("LoggedIn");
            }
            else
            {
                ModelState.AddModelError("", "UserName or Password does not match.");
            }

            return View();
        }

        public ActionResult LoggedIn()
        {
            object obj = Session["UserId"];
            if (obj != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

    }
}
    
