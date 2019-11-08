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
        private BingoDbContext db = new BingoDbContext();

        // Check Email Exists or not in DB  
        private bool IsEmailExists(string eMail)
        {
            var IsCheck = db.Users.Where(email => email.EmailId == eMail).FirstOrDefault();
            return IsCheck != null;
        }

        // Check Username Exists or not in DB  
        private bool IsUserNameExists(string uName)
        {
            var IsCheck = db.Users.Where(uname => uname.UserName == uName).FirstOrDefault();
            return IsCheck != null;
        }


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
                User user = db.Users.Find(id);
                return View("Index", user);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult List()
        {
            object obj = Session["UserId"];
            if (obj != null)
            {
                int uId = Int32.Parse(obj.ToString());
                IEnumerable<User> matchedUsers = (from u in db.Users
                                    from m in db.Matches 
                                    where (u.UserId == uId) || (m.SenderId == uId && m.ReceiverId == u.UserId)
                                                            || (m.ReceiverId == uId && m.SenderId == u.UserId)
                                                  select u).ToList();
                IEnumerable<User> users;
                if (matchedUsers.Count() != 0)
                {
                    users = (from u in db.Users select u).ToList();
                    users = users.Except(matchedUsers);
                }
                else
                {
                    users = db.Users.Where(u => u.UserId != uId).ToList();
                }
                return View(users);
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
            var IsExistsEmail = IsEmailExists(user.EmailId);
            if (IsExistsEmail)
            {
                ModelState.AddModelError("EmailExists", "Email Already Exists");
            }

            var IsExistsUserName = IsUserNameExists(user.UserName);
            if (IsExistsUserName)
            {
                ModelState.AddModelError("UsernameExists", "Username Already Exists");
            }
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
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
                User user = db.Users.Find(uId);
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
            if (obj != null)
            {
                int uId = Int32.Parse(obj.ToString());
                var userToUpdate = db.Users.Find(uId);
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
                        db.SaveChanges();

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
            var get_user = db.Users.SingleOrDefault(p => p.UserName == user.UserName
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
                    User user = db.Users.SingleOrDefault(p => p.UserId == uId);
                    user.ProfilePicture = new byte[file1.ContentLength];
                    file1.InputStream.Read(user.ProfilePicture, 0, file1.ContentLength);
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return View("LoggedIn");
            }*/

    }

}