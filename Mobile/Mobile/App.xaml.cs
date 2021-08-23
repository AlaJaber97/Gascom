using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using localizer = Mobile.Utils.LocalizationResourceManager;
using Microsoft.AppCenter.Distribute;

namespace Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            AppCenter.Start("ios={76820194-552d-4bb6-8b7c-d3da7c025d98};android={638407ed-3800-4080-acde-9835b1199dcc};", typeof(Analytics), typeof(Crashes), typeof(Distribute));
            var page= new Views.LoginPage();
            page.SetBinding(VisualElement.FlowDirectionProperty, new Binding(nameof(localizer.FlowDirection), source: localizer.Instance));
            MainPage = new NavigationPage(page);

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
