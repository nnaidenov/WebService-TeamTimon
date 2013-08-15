using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Services.Models
{
    public class MessageModel
    {
        public int UserID { get; set; }
        public int ChatID { get; set; }
        public string Content { get; set; }
    }
}