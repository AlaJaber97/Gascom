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
        public static int TimeSolt = 1 * 60 + 30;
        public static TimeZoneInfo TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Jordan Standard Time");

        public static async Task<IEnumerable<BLL.Models.Customer>> GetAllCustomersAsync()
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var values = await firebase.Child("Customers").OnceSingleAsync<Dictionary<string, BLL.Models.Customer>>();
                var customers = values.Select(item => item.Value);
                return customers;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
