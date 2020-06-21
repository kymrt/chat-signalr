using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chat_signalr.Models
{
    public class UserDto
    {
        public string ConnectionId { get; set; }
        public string Nickname { get; set; }
        public DateTime LoginTime { get; set; }
        public string IPAddress { get; set; }
    }
}
