using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BlackJackApi.Hubs
{
    public class ChatHub : Hub
    {
        public void Subscribe(string groupName)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public void Unsubscribe(string groupName)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public void SendMessage(string gameid, string senderName, string message, int seatnumber)
        {
            Clients.Group(gameid).SendAsync("sendMessage",senderName, message, seatnumber);
        }

        public void ShowPlayerIsTyping(string gameid, int seatnumber)
        {
            Clients.Group(gameid).SendAsync("showPlayerIsTyping", seatnumber);
        }
    }
}