using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Mobile.Utils
{
    public class FirebaseDatabase
    {
        private static FirebaseDatabase _Instance { get; set; }
        public static FirebaseDatabase Instance => _Instance ??= new FirebaseDatabase();

        public FirebaseClient Client { get; set; }
        private FirebaseDatabase() 
        {
            Client = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
        }

        public async Task<IEnumerable<BLL.Models.Customer>> GetAllCustomersAsync()
        {
            try
            {
                var values = await Client.Child("Customers").OnceSingleAsync<Dictionary<string,BLL.Models.Customer>>();
                return values.Select(item=>item.Value);
            }
            catch (Exception ex)
            {
                Diagnostic.Log(ex);
                return null;
            }
        }
        public async Task<IEnumerable<BLL.Models.Customer>> GetCustomersAsync(string Region)
        {
            try
            {
                var Username = await Utils.LocalStorage.GetUserNameAsync();
                var AllCustomers = await GetAllCustomersAsync();
                var CustomersInRegion = AllCustomers
                                        .Where(item=>item.OrderState == 1)
                                        .Where(item=> !item.IsBooked || (item.BookedBy == Username))
                                        .Where(item => item.Precinct.ToLower() == Region.ToLower());
                return CustomersInRegion;
            }
            catch (Exception ex)
            {
                Diagnostic.Log(ex);
                return null;
            }
        }
        public async Task<IEnumerable<string>> GetRegionsAsync()
        {
            try
            {
                var Countries = await Client.Child("Countries").OnceAsync<Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>>();
                return Countries
                        .Select(item=>item.Object)
                            .SelectMany(item=>item.Values)
                                .SelectMany(item=>item.Values)
                                    .SelectMany(item=>item.Keys);
            }
            catch (Exception ex)
            {
                Diagnostic.Log(ex);
                return null;
            }
        }
    }
}
