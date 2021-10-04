using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IHubContext<SignalRService> _hubContext;
        public UsersController(IHubContext<SignalRService> hubContext)
        {
            _hubContext = hubContext;
        }
        [HttpGet]
        public async Task CheckUserStatus()
        {
            try
            {
                var firebase = new Firebase.Database.FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var customers = await FirebaseService.GetAllCustomersAsync();
                foreach (var customer in customers.Where(item => item.IsBooked))
                {
                    var timeNow = TimeZoneInfo.ConvertTime(DateTime.Now, FirebaseService.TimeZoneInfo);
                    var timeAgo = timeNow.Subtract(customer.BookedAt);
                    if (timeAgo.TotalMinutes > FirebaseService.TimeSolt)
                    {
                        var username = customer.BookedBy.ToString();
                        customer.BookedAtStr = string.Empty;
                        customer.BookedBy = string.Empty;
                        await firebase.Child($"Customers/{customer.ID}").PutAsync(Newtonsoft.Json.JsonConvert.SerializeObject(customer));

                        SignalRService.Connections.TryGetValue(username, out string connectionToSendMessage);
                        if (!string.IsNullOrWhiteSpace(connectionToSendMessage))
                        {
                            await _hubContext.Clients.Client(connectionToSendMessage).SendAsync("UpdateStatusUser");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
