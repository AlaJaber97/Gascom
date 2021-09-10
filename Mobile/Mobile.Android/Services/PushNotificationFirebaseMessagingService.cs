using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using Mobile.Interface;
using Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobile.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class PushNotificationFirebaseMessagingService : FirebaseMessagingService
    {
        IPushNotificationActionService _notificationActionService;
        INotificationRegistrationService _notificationRegistrationService;
        IDeviceInstallationService _deviceInstallationService;

        IPushNotificationActionService NotificationActionService
            => _notificationActionService ??=
                ServiceContainer.Resolve<IPushNotificationActionService>();

        INotificationRegistrationService NotificationRegistrationService
            => _notificationRegistrationService ??=
                ServiceContainer.Resolve<INotificationRegistrationService>();

        IDeviceInstallationService DeviceInstallationService
            => _deviceInstallationService ??=
                ServiceContainer.Resolve<IDeviceInstallationService>();

        public override void OnNewToken(string token)
        {
            DeviceInstallationService.Token = token;

            NotificationRegistrationService.RefreshRegistrationAsync()
                .ContinueWith((task) => { if (task.IsFaulted) throw task.Exception; });
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            if (message.Data.TryGetValue("action", out var messageAction))
                NotificationActionService.TriggerAction(messageAction);
        }
    }
}