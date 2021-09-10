using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using localizer = Mobile.Utils.LocalizationResourceManager;


namespace Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        public async Task DisplayAlertAsync(string Title,string Message, string CancelButton)
        {
            await Application.Current.MainPage.DisplayAlert(Title, Message, CancelButton);
        }

        public async Task DisplayToastAsync(string Message)
        {
            await MaterialDialog.Instance.SnackbarAsync(Message,  MaterialSnackbar.DurationLong, Configration.MaterialConfigration.SnackbarDialogConfiguration);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
        protected void OnPropretyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
