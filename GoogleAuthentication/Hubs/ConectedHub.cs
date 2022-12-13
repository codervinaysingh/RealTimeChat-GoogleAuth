using GoogleAuthentication.Models;
using GoogleAuthentication.Models.ChatHistory;
using GoogleAuthentication.Models.UserMaster;
using Microsoft.AspNetCore.SignalR;

namespace GoogleAuthentication.Hubs
{
    public class ConectedHub : Hub
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserMaster _usermaster;
        private readonly IChatHistory _chat;
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();
        UserMaster obj = new UserMaster();
        ChatHistory chatHistory = new ChatHistory();
        public ConectedHub(IHttpContextAccessor httpContext, IUserMaster usermaster, IChatHistory chat)
        {
            _httpContext = httpContext;
            _usermaster = usermaster;
            _chat = chat;
        }
        public override Task OnConnectedAsync()
        {
            obj.UserName = Context.User.Identities.Last().Claims.Last().Value;
            obj.ConnectionId = Context.ConnectionId;
            obj.OpCode = 1;
            var res = _usermaster.AddUpdateConnection(obj);

            _connections.Add(obj.UserName, Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            obj.UserName = Context.User.Identities.Last().Claims.Last().Value;
            obj.ConnectionId = Context.ConnectionId;
            obj.OpCode = 2;
            var res = _usermaster.AddUpdateConnection(obj);
            _connections.Remove(obj.UserName, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            var userId = Context.User.Identities.Last().Claims.Last().Value;
            chatHistory.sendTo = user;
            chatHistory.message = message;
            chatHistory.userName = userId;
            chatHistory.opCode = 1;

            if (string.IsNullOrEmpty(user))
            {
                await Clients.All.SendAsync("ReceiveMessage", userId, message);
            }
            else
            {
                var res = _chat.sendChatHistory(chatHistory);

                foreach (var connectionId in _connections.GetConnections(user))
                {
                    await Clients.Clients(connectionId).SendAsync("ReceiveMessage", res);
                }

            }

        }
    }
}
