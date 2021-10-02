using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using localizer = Mobile.Utils.LocalizationResourceManager;
using Microsoft.AppCenter.Distribute;
using System.Threading.Tasks;
using XF.Material.Forms.Resources;
using Mobile.Interface;
using Xamarin.Essentials;

namespace Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);

            Services.ServiceContainer.Resolve<IPushNotificationActionService>().ActionTriggered += NotificationActionTriggered;
            StartupPage().ConfigureAwait(true);
        }

        private async Task StartupPage()
        {
            Page page;
            if (string.IsNullOrEmpty(await Utils.LocalStorage.GetTokenAsync()))
            {
                page = new Views.LoginPage();
            }
            else if(string.IsNullOrEmpty(await Utils.LocalStorage.GetLocalityAsync()))
            {
                page = new Views.LocalityPage();
            }
            else
            {
                page = new NavigationPage(new Views.MainPage());
            }

            page.SetBinding(VisualElement.FlowDirectionProperty, new Binding(nameof(localizer.FlowDirection), source: localizer.Instance));
            MainPage = page;
        }
        void NotificationActionTriggered(object sender, BLL.Enums.PushNotificationAction action)
        {
            switch (action)
            {
                case BLL.Enums.PushNotificationAction.UpdateUser:
                    MessagingCenter.Send(this, "UpdateStatusUser");
                    break;
                default:
                    break;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
