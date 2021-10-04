using Firebase.Database;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class SignalRService : Hub
    {
        public static ConcurrentDictionary<string, string> Connections = new ConcurrentDictionary<string, string>();

        public async Task SendNotificationCancelReversedUser(string username)
        {
            Connections.TryGetValue(username, out string connectionToSendMessage);

            if (!string.IsNullOrWhiteSpace(connectionToSendMessage))
            {
                await Clients.Client(connectionToSendMessage).SendAsync("UpdateStatusUser");
            }
        }
        public override Task OnConnectedAsync()
        {
            if (!Connections.ContainsKey(Context.ConnectionId))
            {
                var httpContext = Context.GetHttpContext();
                httpContext.Request.Query.TryGetValue("UserName", out StringValues value);
                string UserName = value.ToString();
                if (!string.IsNullOrWhiteSpace(UserName))
                {
                    Connections.AddOrUpdate(UserName, Context.ConnectionId, (key, oldValue) => Context.ConnectionId);
                }
            }

            return base.OnConnectedAsync();
        }
    }
}
