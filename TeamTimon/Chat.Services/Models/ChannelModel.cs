using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Services.Models
{
    public class ChannelModel
    {
        public string ChannelName { get; set; }
        public int FirstUserId { get; set; }
        public int SecondUserId { get; set; }
    }
}