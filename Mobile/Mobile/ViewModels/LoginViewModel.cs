using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Mobile.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using localizer = Mobile.Utils.LocalizationResourceManager;

namespace Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public string UserName { get; set;}
        public string Password { get; set;}

        private Command loginCommand;
        public ICommand LoginCommand => loginCommand ??= new Command(async()=> await Login().ConfigureAwait(false));

        private async Task Login()
        {
            try
            {
                if (!CanLogin)
                {
                    await DisplayToastAsync("يرجى ادخال البريد الالكتروني وكلمة المرور...").ConfigureAwait(false);
                }
                else
                {
                    using (await XF.Material.Forms.UI.Dialogs.MaterialDialog.Instance.LoadingDialogAsync($"{localizer.Instance["Login"]}...", Configration.MaterialConfigration.LoadingDialogConfiguration))
                    {
                        var firebaseToken = await LoginWithEmailPasswordAsync(this.UserName, this.Password);
                        if (string.IsNullOrEmpty(firebaseToken)) return;
                        await Utils.LocalStorage.SetUserNameAsync(this.UserName);
                        await Utils.LocalStorage.SetTokenAsync(firebaseToken);
                    }
                    var permission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (permission != PermissionStatus.Granted)
                    {
                        await DisplayAlertAsync(localizer.Instance["PermissionRequest"], localizer.Instance["WhyWeNeedThisPermission"], localizer.Instance["Okay"]);
                        await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    }
                    Application.Current.MainPage = new Views.LocalityPage();
                }
            }
            catch(Exception ex)
            {
                await DisplayToastAsync("حدث خطأ ما, يرجى تأكد من اتصالك بالانترنت ومحاولة لاحقاً");
                Utils.Diagnostic.Log(ex, "Login Procdure");
            }
        }

        //private Command forgetPasswordCommand;
        //public ICommand ForgetPasswordCommand => forgetPasswordCommand ??= new Command(async () => await ForgetPassword().ConfigureAwait(false));

        //private async Task ForgetPassword()
        //{

        //}

        public bool CanLogin => !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        public async Task<string> LoginWithEmailPasswordAsync(string email, string password)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(BLL.Configration.FirebaseConfigration.ApiKey));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);

                return auth.FirebaseToken;
            }
            catch(FirebaseAuthException ex)
            {
                if (ex.ResponseData.Contains("EMAIL_NOT_FOUND"))
                    await DisplayToastAsync("يبدو ان هذه البريد الالكتروني غير موجود في قاعدة البيانات");
                else if (ex.ResponseData.Contains("INVALID_PASSWORD"))
                    await DisplayToastAsync("يبدو ان كلمة المرور غير صحيحة");
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
