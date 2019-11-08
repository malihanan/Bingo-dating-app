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
                Match find = db.Matches.SingleOrDefault(m => m.SenderId == id && m.ReceiverId == uId);
                if (find != null)
                {
                    find.ReceiverResult = result;
                }
                else
                {
                    db.Matches.Add(new Match()
                    {
                        SenderId = uId,
                        ReceiverId = id,
                        SenderResult = result,
                        SenderTime = DateTime.Now
                    });
                }
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
                                    where m.SenderResult == true
                                    orderby m.SenderTime descending
                                    select new Notification
                                    {
                                        UserName = u2.UserName,
                                        FirstName = u2.FirstName,
                                        LastName = u2.LastName,
                                        ProfilePicture = u2.ProfilePicture,
                                        Bio = u2.Bio,
                                        UserId = m.SenderId,
                                        Time = m.SenderTime,
                                        Result = m.ReceiverResult
                                    }).ToList();
                ViewBag.direction = "Received";
                return View("Notifications", notifications);
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }
        }
        public ActionResult NotificationsSent()
        {
            object obj = Session["UserId"];
            if (obj != null)
            {
                int uId = Int32.Parse(obj.ToString());
                List<Notification> notifications = (from m in db.Matches
                                                    join u in db.Users on m.SenderId equals u.UserId
                                                    from u2 in db.Users
                                                    where u2.UserId == m.ReceiverId
                                                    where m.SenderId == uId
                                                    where m.SenderResult == true
                                                    orderby m.SenderTime descending
                                                    select new Notification
                                                    {
                                                        UserName = u2.UserName,
                                                        FirstName = u2.FirstName,
                                                        LastName = u2.LastName,
                                                        ProfilePicture = u2.ProfilePicture,
                                                        Bio = u2.Bio,
                                                        UserId = m.SenderId,
                                                        Time = m.SenderTime,
                                                        Result = m.ReceiverResult
                                                    }).ToList();
                ViewBag.direction = "Sent";
                return View("Notifications", notifications);
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }
        }
    }
}