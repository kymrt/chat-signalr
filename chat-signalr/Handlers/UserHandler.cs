using chat_signalr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chat_signalr.Handlers
{
    public class UserHandler
    {
        public List<UserDto> Users = new List<UserDto>();
        public static readonly object Locker = new object();

        public bool Add(string connectionId, string nickname, string ipAdress)
        {
            lock (Locker)
            {
                bool result = false;
                var user = Users.SingleOrDefault(u => u.Nickname == nickname);
                if(user == null)
                {
                    Users.Add(new UserDto()
                    {
                        ConnectionId = connectionId,
                        Nickname = nickname,
                        LoginTime = DateTime.Now,
                        IPAddress = ipAdress
                    });
                    result = true;
                }
                return result;
            }
        }

        public void Remove(string connectionId)
        {
            lock (Locker)
            {
                var user = Users.SingleOrDefault(u => u.ConnectionId == connectionId);
                if(user != null)
                {
                    Users.Remove(user);
                }
            }
        }

        public string GetNickname(string connectionId)
        {
            lock (Locker)
            {
                var user = Users.SingleOrDefault(u => u.ConnectionId == connectionId);
                if (user != null)
                {
                    return user.Nickname;
                }
            }
            return null;
        }

    }
}
