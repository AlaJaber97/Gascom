using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set;}
        public string Password { get; set;}

        private Command loginCommand;
        public ICommand LoginCommand => loginCommand ??= new Command(async()=> await Login().ConfigureAwait(false));

        private async Task Login()
        {
            if (!CanLogin) return;
            var firebaseToken = await LoginWithEmailPasswordAsync(this.UserName, this.Password);
            if (string.IsNullOrEmpty(firebaseToken)) return;
            await Xamarin.Essentials.SecureStorage.SetAsync("FirebaseToken", firebaseToken);
        }

        private Command forgetPasswordCommand;
        public ICommand ForgetPasswordCommand => forgetPasswordCommand ??= new Command(async () => await ForgetPassword().ConfigureAwait(false));

        private async Task ForgetPassword()
        {

        }

        public bool CanLogin => !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        public async Task<string> LoginWithEmailPasswordAsync(string email, string password)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Mobile.Configration.FirebaseConfigration.ApiKey));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);

                return auth.FirebaseToken;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
