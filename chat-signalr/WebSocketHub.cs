using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using System.Web;

namespace chat_signalr
{
    public class WebSocketHub : Hub
    {
        public void Login(string nickName, string ipAdress)
        {
            if(Program.UserHandler.Add(Context.ConnectionId, nickName, ipAdress))
            {
                Clients.Client(Context.ConnectionId).SendAsync("Login", true);
            }
            else
            {
                Clients.Client(Context.ConnectionId).SendAsync("Error", "Existing nickname");
            }
        }
        
        public void Logout()
        {
            Program.UserHandler.Remove(Context.ConnectionId);            
             Clients.Client(Context.ConnectionId).SendAsync("Logout", true);
            
        }

        public void Message(string message)
        {
            //We get the nickname from the system with connectionId to avoid of manipulating nickname.
            var nickname = Program.UserHandler.GetNickname(Context.ConnectionId);
            if(nickname != null)
            {
                //We use htmlencode to avodi of manipulating with message that are used html tag.
                int id = Program.MessageHandler.Add(nickname, HttpUtility.HtmlEncode(message));
                //We can pass as many parameters as we want, we just should recieve these in frontend.
                string messageTime = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                Clients.Client(Context.ConnectionId).SendAsync("MessageMine", id, nickname, message, messageTime);
                Clients.AllExcept(Context.ConnectionId).SendAsync("MessageOther", id, nickname, message, messageTime);
            }
            else
            {
                Clients.Client(Context.ConnectionId).SendAsync("Error", "Somethings went wrong.");
            }
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Connected: " + Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("Disconnected: " + Context.ConnectionId);
            //We remove the user from system, so someone can take a same nickname.
            Program.UserHandler.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
