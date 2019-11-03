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
    }
}