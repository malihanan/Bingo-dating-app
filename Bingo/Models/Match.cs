using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bingo.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        [Required]
        public int SenderId { get; set; }
        [Required]
        public int ReceiverId { get; set; }
        [Required]
        public bool SenderResult { get; set; }
        [Required]
        public DateTime? SenderTime { get; set; }
        public bool? ReceiverResult { get; set; }
        public DateTime? ReceiverTime { get; set; }
    }
}