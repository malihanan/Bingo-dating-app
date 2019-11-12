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
                                                        UserId = m.ReceiverId,
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
        public ActionResult ShowChats()
        {
            object obj = Session["UserId"];
            if (obj != null)
            {
                int uId = Int32.Parse(obj.ToString());
                List<User> users = (from u in db.Users
                                    from c in db.Conversations
                                    where c.sender_id == uId || c.receiver_id == uId
                                    orderby c.created_at
                                    select u).Distinct().ToList();
                return View(users);
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }
        }
        public ActionResult ConversationWithContact(int contact)
        {
            object obj = Session["UserId"];
            Session["Receiver"] = contact.ToString();
            if (obj != null)
            {
                int uId = Int32.Parse(obj.ToString());

                IEnumerable<Conversation> conversations = (from c in db.Conversations
                                                           where (c.receiver_id == uId && c.sender_id == contact) || (c.receiver_id == contact && c.sender_id == uId)
                                                           orderby c.created_at ascending
                                                           select c).ToList();
                ViewBag.currentUser = uId;
                return View(conversations);
            }
            else
            {
                return RedirectToAction("Login", "User", null);

            }
        }
        [HttpPost]
        public ActionResult ConversationWithContact(String messages)
        {
            object obj = Session["UserId"];
            object obj2 = Session["Receiver"];
            if (obj != null)
            {

                int uId = Int32.Parse(obj.ToString());
                int rId = Int32.Parse(obj2.ToString());

                db.Conversations.Add(new Conversation()
                {
                    sender_id = uId,
                    message = messages,
                    receiver_id = rId,
                    created_at = DateTime.Now
                });
                db.SaveChanges();

                IEnumerable<Conversation> conversations = (from c in db.Conversations
                                                           where (c.receiver_id == uId && c.sender_id == rId) || (c.receiver_id == rId && c.sender_id == uId)
                                                           orderby c.created_at ascending
                                                           select c).ToList();
                System.Console.Write(uId.ToString(), rId);
                ViewBag.currentUser = uId;
                return View(conversations);
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }
        }
    }
}