using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Mobile.Interface;
using Mobile.Services;
using Android.Content;
using Mobile.Droid.Services;
using Firebase.Iid;
using AndroidX.AppCompat.App;
using Android.Gms.Tasks;

namespace Mobile.Droid
{
    [Activity(Theme = "@style/splashscreen",
        LaunchMode = LaunchMode.SingleTop, 
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
        , Android.Gms.Tasks.IOnCompleteListener , IApplication
    //, Android.Gms.Tasks.IOnSuccessListener
    {
        public static Context Instance { get; internal set; }
        IPushNotificationActionService _notificationActionService;
        IDeviceInstallationService _deviceInstallationService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.Window.RequestFeature(Android.Views.WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(savedInstanceState); 
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Utils.Bootstrap.Begin(() => new DeviceInstallationService());
            if (DeviceInstallationService.NotificationsSupported)
            {
                Firebase.Messaging.FirebaseMessaging.Instance
                    .GetToken().AddOnCompleteListener(this);
                //FirebaseInstanceId.GetInstance(Firebase.FirebaseApp.Instance)
                //    .GetInstanceId().AddOnSuccessListener(this);
                //FirebaseInstanceId.GetInstanceId().AddOnCompleteListener(this);
            }
            AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.Start("638407ed-3800-4080-acde-9835b1199dcc", typeof(Analytics), typeof(Crashes), typeof(Distribute));
            XF.Material.Droid.Material.Init(this, savedInstanceState);
            Distribute.CheckForUpdate();
            Distribute.SetEnabledAsync(true);
            Instance = this;
            LoadApplication(new App());
            ProcessNotificationActions(Intent);
        }

        IPushNotificationActionService NotificationActionService
            => _notificationActionService ??
                (_notificationActionService =
                ServiceContainer.Resolve<IPushNotificationActionService>());

        IDeviceInstallationService DeviceInstallationService
            => _deviceInstallationService ??
                (_deviceInstallationService =
                ServiceContainer.Resolve<IDeviceInstallationService>());


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        //public void OnSuccess(Java.Lang.Object result)
        //    => DeviceInstallationService.Token =
        //        result.Class.GetMethod("getToken").Invoke(result).ToString();
        public void OnComplete(Task token)
        {
            DeviceInstallationService.Token =
                token.Class.GetMethod("getResult").Invoke(token).ToString();
            //Xamarin.Forms.MessagingCenter.Send(this, "RegisterationDevices");
            Mobile.ViewModels.MainViewModel.RegisterationDevices().ConfigureAwait(false);
        }
        
        void ProcessNotificationActions(Intent intent)
        {
            try
            {
                //if (intent?.HasExtra("text") == true)
                //{
                //    var message = intent.GetStringExtra("text");

                //    if (!string.IsNullOrEmpty(message))
                //        NotificationActionService.TriggerMessageRecived(message);
                //}
                if (intent?.HasExtra("action") == true)
                {
                    var action = intent.GetStringExtra("action");

                    if (!string.IsNullOrEmpty(action))
                        NotificationActionService.TriggerAction(action);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            ProcessNotificationActions(intent);
        }

        
    }
}