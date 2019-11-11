using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bingo.Models
{
    public class Conversation
    {
        public int id { get; set; }
        public int sender_id { get; set; }
        public int receiver_id { get; set; }
        public string message { get; set; }
        public DateTime created_at { get; set; }

    }
}