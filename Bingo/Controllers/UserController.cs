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
        public ActionResult Index(int? id)
        {
            object obj = Session["UserId"];
            if (obj != null)
            {
                if(id == null)
                {
                    id = Int32.Parse(obj.ToString());
                }
                User user = ctx.Users.Find(id);
                return View("Index", user);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult List()
        {
            if (Session["Role"] != null && Session["Role"].ToString() == "admin")
            {
                return View(ctx.Users.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home", null);
            }
        }

        //Signup
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(User user)
        {
            if (ModelState.IsValid)
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }
        //Edit User
        public ActionResult Edit()
        {
            object obj = Session["UserId"];
            if (obj != null)
            {
                int uId = Int32.Parse(obj.ToString());
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
                if (TryUpdateModel(userToUpdate, "", new string[] {
                    "UserName", "FirstName", "LastName", "Gender", "DisplayBirthdate", "City", "Occupation",
                    "ProfilePicture", "Bio", "Likes", "Dislikes", "Hobbies", "Contact" }))
                {
                    try
                    {
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            userToUpdate.ProfilePicture = new byte[file1.ContentLength];
                            file1.InputStream.Read(userToUpdate.ProfilePicture, 0, file1.ContentLength);
                        }
                        ctx.SaveChanges();

                        return RedirectToAction("Index", "User", null);
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                        throw e;
                    }
                }
            }
            return View(User);
        }
        //Login
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
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "UserName or Password does not match.");
            }

            return View();
        }
        public ActionResult Logout()
        {

            object obj = Session["UserId"];
            if (obj != null)
            {
                Session.Clear();
            }
            return RedirectToAction("Index", "Home", null);
        }

        /*
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
            }*/

    }

}