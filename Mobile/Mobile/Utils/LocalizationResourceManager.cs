using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using Mobile.Languages;

namespace Mobile.Utils
{
    public class LocalizationResourceManager : INotifyPropertyChanged
    {
        public static LocalizationResourceManager Instance { get; private set; }
        public FlowDirection FlowDirection { get; set; }
        private LocalizationResourceManager() { }
        public static void Initialization()
        {
            Instance ??= new LocalizationResourceManager();
        }

        public void SetCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            Resource.Culture = culture;
            FlowDirection = culture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            //how rememebr culture selected after close app

            Invalidate();
        }

        public string this[string key] => GetValue(key);
        public string GetValue(string key)
        {
            return Resource.ResourceManager.GetString(key, Resource.Culture);
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
