using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bingo.Models;

namespace Bingo.Controllers
{
    public class MatchController : Controller
    {
        private BingoDbContext db = new BingoDbContext();
        // GET: Match
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Send(int id, bool result)
        {
            object obj = Session["UserId"];
            if(obj != null)
            {
                int uId = Int32.Parse(obj.ToString());
                db.Matches.Add(new Match()
                {
                    SenderId = uId,
                    ReceiverId = id,
                    SenderResult = result,
                    SenderTime = DateTime.Now
                });
                db.SaveChanges();
                return RedirectToAction("List", "User", null);
            }
            return HttpNotFound();
        }

        public ActionResult NotificationsReceived()
        {
            object obj = Session["UserId"];
            if (obj != null)
            {
                int uId = Int32.Parse(obj.ToString());
                List<Notification> notifications = (from m in db.Matches
                                    join u in db.Users on m.ReceiverId equals u.UserId
                                    from u2 in db.Users
                                    where u2.UserId == m.SenderId
                                    where m.ReceiverId == uId
                                    select new Notification
                                    {
                                        UserName = u2.UserName,
                                        FirstName = u2.FirstName,
                                        LastName = u2.LastName,
                                        ProfilePicture = u2.ProfilePicture,
                                        Bio = u2.Bio,
                                        SenderId = m.SenderId,
                                        SenderTime = m.SenderTime
                                    }).ToList();
                return View(notifications);
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }
        }
    }
}