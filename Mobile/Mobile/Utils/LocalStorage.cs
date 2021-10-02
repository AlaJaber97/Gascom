using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Utils
{
    public static class LocalStorage
    {
        private static string FirebaseToken => "FirebaseToken";
        private static string Locality => "Locality";
        private static string Username => "Username";
        public const string CachedDeviceTokenKey = "cached_device_token";
        public const string CachedTagsKey = "cached_tags";

        public static async Task<string> GetTokenAsync()
        {
            return await Xamarin.Essentials.SecureStorage.GetAsync(FirebaseToken);
        }
        public static async Task SetTokenAsync(string firebaseToken)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync(FirebaseToken, firebaseToken);
        }
        public static async Task<string> GetLocalityAsync()
        {
            return await Xamarin.Essentials.SecureStorage.GetAsync(Locality);
        }
        public static async Task SetLocalityAsync(string locality)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync(Locality, locality);
        }

        public static async Task<string> GetUserNameAsync()
        {
            return await Xamarin.Essentials.SecureStorage.GetAsync(Username);
        }
        public static async Task SetUserNameAsync(string userName)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync(Username, userName);
        }
    }
}
