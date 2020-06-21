using chat_signalr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chat_signalr.Handlers
{
    public class MessageHandler
    {
        public List<MessageDto> Messages = new List<MessageDto>();
        //We define this to avoid locking system when message are sent to the system at the same time.
        private static readonly object Locker = new object();


        public int Add(string nickname, string message)
        {
            //We set 1 to id in the first. We mash it, if there is a message in the system.
            int id = 1;
            lock (Locker)
            {
                var lastMessage = Messages.LastOrDefault();
                if (lastMessage != null)
                    id = lastMessage.Id + 1;
                Messages.Add(new MessageDto()
                {
                    Id = id,
                    Nickname = nickname,
                    Message = message,
                    MessageTime = DateTime.Now
                });
            }
            return id;
        }
    }
}
