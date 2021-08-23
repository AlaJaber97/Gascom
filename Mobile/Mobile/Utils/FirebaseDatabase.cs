using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Utils
{
    public class FirebaseDatabase
    {
        private static FirebaseDatabase _Instance { get; set; }
        public static FirebaseDatabase Instance => _Instance ??= new FirebaseDatabase();

        private static FirebaseClient Client { get; set; }
        private FirebaseDatabase() 
        {
            Client = new FirebaseClient(Mobile.Configration.FirebaseConfigration.DatabaseURL);
        }

        private async Task<IEnumerable<BLL.Models.Customer>> GetCustomersAsync()
        {
            try
            {
                var dbData = await Client.Child("Customer").OnceAsync<BLL.Models.Customer>();
                var Customers = dbData.Select(item =>
                {
                    item.Object.PhoneNumber = item.Key;
                    return item.Object;
                });
                return Customers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
