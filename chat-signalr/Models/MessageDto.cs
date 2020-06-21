using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chat_signalr.Models
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Message { get; set; }
        public DateTime MessageTime { get; set; }
    }
}
