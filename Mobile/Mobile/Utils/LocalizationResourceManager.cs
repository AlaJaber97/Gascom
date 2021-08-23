using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace Mobile.Utils
{
    public class LocalizationResourceManager : INotifyPropertyChanged
    {
        private static LocalizationResourceManager _Instance { get; set; }
        public static LocalizationResourceManager Instance => _Instance ??= new LocalizationResourceManager();
        public FlowDirection FlowDirection { get; set; }
        private LocalizationResourceManager() { }

        public void SetCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            Mobile.Languages.Resource.Culture = culture;
            FlowDirection = culture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            //how rememebr culture selected after close app

            Invalidate();
        }

        public string this[string key] => GetValue(key);
        public string GetValue(string key)
        {
            var value = Mobile.Languages.Resource.ResourceManager.GetString(key, Mobile.Languages.Resource.Culture);
            return value ?? key;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void Invalidate()
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
            catch (Exception)
            {

            }
        }
    }
}
