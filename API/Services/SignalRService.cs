using Firebase.Database;
using Microsoft.AspNetCore.SignalR;
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
            string connectionToSendMessage;
            Connections.TryGetValue(username, out connectionToSendMessage);

            if (!string.IsNullOrWhiteSpace(connectionToSendMessage))
            {
                await Clients.Client(connectionToSendMessage).SendAsync("UpdateStatusUser");
            }
        }
        public async Task CheckUserStatus()
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var customers = await FirebaseService.GetAllCustomersAsync();
                foreach (var customer in customers.Where(item => item.IsBooked))
                {
                    var timeNow = TimeZoneInfo.ConvertTime(DateTime.Now, FirebaseService.TimeZoneInfo);
                    var timeAgo = timeNow.Subtract(customer.BookedAt);
                    if (timeAgo.TotalMinutes > FirebaseService.TimeSolt)
                    {
                        var userName = customer.BookedBy.ToString();
                        customer.BookedAtStr = string.Empty;
                        customer.BookedBy = string.Empty;
                        await firebase.Child($"Customers/{customer.ID}").PutAsync(JsonConvert.SerializeObject(customer));
                        await SendNotificationCancelReversedUser(userName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override Task OnConnectedAsync()
        {
            if (!Connections.ContainsKey(Context.ConnectionId))
            {
                Context.Items.TryGetValue("UserName", out object value);
                string UserName = value?.ToString();
                if (!string.IsNullOrWhiteSpace(UserName))
                {
                    Connections.AddOrUpdate(UserName, Context.ConnectionId, (key, oldValue) => Context.ConnectionId);
                }
            }

            return base.OnConnectedAsync();
        }
    }
}
