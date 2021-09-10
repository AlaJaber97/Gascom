using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public static class FirebaseService
    {
        /// <summary>
        /// Editing requests that have been booked for more than an hour and a half
        /// </summary>
        public static async Task CheckUserStatus()
        {
            var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
            var customers = await firebase.Child("Customers").OnceSingleAsync<List<BLL.Models.Customer>>();
            foreach (var customer in customers.Where(item=> item.IsBooked))
            {
                if(DateTime.Now.Subtract(customer.BookedAt) > new TimeSpan(1, 30, 0))
                {
                    customer.BookedAtStr = string.Empty;
                    customer.BookedBy = string.Empty;
                    await firebase.Child($"Customers/{customer.ID}").PutAsync(JsonConvert.SerializeObject(customer));
                }
            }
        }
    }
}
