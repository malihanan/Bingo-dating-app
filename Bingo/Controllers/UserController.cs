using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bingo.Models;

namespace Bingo.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            using (var ctx = new BingoDbContext())
            {
                return View(ctx.Users.ToList());
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            using (var ctx = new BingoDbContext())
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }
            return View("List");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            using (BingoDbContext db = new BingoDbContext())
            {
                var get_user = db.Users.SingleOrDefault(p => p.UserName == user.UserName
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
    
