using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bingo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "GetContactConversations",
                url: "contact/conversations/{contact}",
                defaults: new { controller = "Match", action = "ConversationWithContact", contact = "" }
            );

            routes.MapRoute(
                name: "SendMessage",
                url: "send_message",
                defaults: new { controller = "Match", action = "SendMessage" }
            );
        }
    }
}
