using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bingo.Models
{
    public class Notification
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string Bio { get; set; }
        public DateTime? Time { get; set; }
        public bool? Result { get; set; }
    }
}