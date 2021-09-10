using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using localizer = Mobile.Utils.LocalizationResourceManager;

namespace Mobile.ViewModels
{
    public class LocalityViewModel : BaseViewModel
    {
        public ObservableCollection<string> Regions { get; set; }
        public string RegionSelected { get; set; }
        public LocalityViewModel()
        {
            Regions = new ObservableCollection<string>();
            Initialization();
        }
        private async void Initialization()
        {
            try
            {
                using (await XF.Material.Forms.UI.Dialogs.MaterialDialog.Instance.LoadingDialogAsync($"{localizer.Instance["Get Regions"]}...", Configration.MaterialConfigration.LoadingDialogConfiguration))
                {
                    var regions = await Utils.FirebaseDatabase.Instance.GetRegionsAsync();
                    if (regions != null) Regions = new ObservableCollection<string>(regions);
                    OnPropretyChanged(nameof(Regions));
                }
            }
            catch (Exception ex)
            {
                Utils.Diagnostic.Log(ex);
            }
        }

        private Command doneCommand;
        public ICommand DoneCommand => doneCommand ??= new Command(async()=> await Done().ConfigureAwait(false));

        private async Task Done()
        {
            try
            {

                await Utils.LocalStorage.SetLocalityAsync(RegionSelected).ConfigureAwait(false);
                Application.Current.MainPage = new NavigationPage(new Views.MainPage());
            }
            catch (Exception ex)
            {
                Utils.Diagnostic.Log(ex);
            }
        }
    }
}
