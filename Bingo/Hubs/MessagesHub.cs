using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Bingo.Hubs
{
    [HubName("messagesHub")]
    public class MessagesHub : Hub
    {
        public static void BroadcastData()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();
            context.Clients.All.refreshMessageData();
        }
    }
}