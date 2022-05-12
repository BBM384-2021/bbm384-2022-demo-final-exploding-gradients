using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LinkedHU_CENG.Hubs
{
    public class ChatHub : Hub
    {

        public Task SendMessageToUser(string senderId, string receiverId, string message)
        {
            //based on the receiver name to query the database and get the connection id

            return Clients.Client("connectionId").SendAsync("ReceiveMessage", senderId, message);
        }
    }
}
