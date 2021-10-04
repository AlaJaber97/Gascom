using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using localizer = Mobile.Utils.LocalizationResourceManager;
using System.Threading;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Mobile.Interface;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using BLL.Extensions;

namespace Mobile.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public HubConnection _hubConnection;
        public string UserName { get; set; }
        private bool _isLoadingData;
        public bool IsLoadingData
        {
            get { return _isLoadingData; }
            set { SetProperty(ref _isLoadingData, value); }
        }

        private ObservableCollection<BLL.Models.Customer> _Customers;
        public ObservableCollection<BLL.Models.Customer> Customers
        {
            get { return _Customers ??= new ObservableCollection<BLL.Models.Customer>(); }
            set { SetProperty(ref _Customers, value); }
        }

        public MainViewModel()
        {
            LoadCustomer().ConfigureAwait(false);
            InitHub().ConfigureAwait(false);
            MessagingCenter.Subscribe<App>(this, "UpdateStatusUser", async (sender) =>
            {
                await LoadCustomer().ConfigureAwait(false);
            });

            
        }
        private async Task InitHub()
        {
            UserName = await Utils.LocalStorage.GetUserNameAsync();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{BLL.Settings.Configration.HubServerAddress}?UserName={UserName}")
                .WithAutomaticReconnect().Build();

            _hubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _hubConnection.StartAsync();
            };

            await Connect().ConfigureAwait(false);
        }
        private async Task Connect()
        {
            if (_hubConnection.State == HubConnectionState.Connected) return;

            _hubConnection.On("UpdateStatusUser", async () => await LoadCustomer().ConfigureAwait(false));

            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Utils.Diagnostic.Log(ex, "Try Connect with gascom-signalR");
            }
        }
        private async Task Disconnect()
        {
            await _hubConnection.DisposeAsync();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{BLL.Settings.Configration.HubServerAddress}?UserName={UserName}")
                .Build();

            await Connect();
        }
        public static async Task RegisterationDevices()
        {
            try
            {
                var EmailUser = await Utils.LocalStorage.GetUserNameAsync();
                var Locality = await Utils.LocalStorage.GetLocalityAsync();

                var tags = new List<string>();
                tags.Add("gascom".Validation());
                if (!string.IsNullOrEmpty(EmailUser)) tags.Add(EmailUser.Validation());
                if (!string.IsNullOrEmpty(Locality)) tags.Add(Locality.Validation());

                var _notificationRegistrationService = Services.ServiceContainer.Resolve<INotificationRegistrationService>();
                await _notificationRegistrationService.RegisterDeviceAsync(tags.ToArray());
            }
            catch (Exception ex)
            {
                Utils.Diagnostic.Log(ex);
            }
        }

        private Command loadCustomerCommand;
        public ICommand LoadCustomerCommand => loadCustomerCommand ??= new Command(() => LoadCustomer().ConfigureAwait(false));

        public async Task LoadCustomer()
        {
            try
            {
                var Message = $"{localizer.Instance["Get Customers"]}...";
                if (Customers != null && Customers.Count > 0) Message = $"{localizer.Instance["Update Status Customer"]}...";
                using (await XF.Material.Forms.UI.Dialogs.MaterialDialog.Instance.LoadingDialogAsync(Message, Configration.MaterialConfigration.LoadingDialogConfiguration))
                {
                    var RegionOperation = await Utils.LocalStorage.GetLocalityAsync();
                    var _Customers = await Utils.FirebaseDatabase.Instance.GetCustomersAsync(RegionOperation);
                    if (_Customers != null)
                    {
                        var ListOfCustomer = _Customers.OrderByDescending(item => item.OrderState);

                        var permission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                        if (permission == PermissionStatus.Granted)
                        {
                            var CurrentLocation = await GetCurrentLocation();
                            if (CurrentLocation != null)
                                ListOfCustomer = ListOfCustomer.ThenBy(item => GetDistance(CurrentLocation, item.Latitude, item.Longtitude));
                        }

                        Customers = new ObservableCollection<BLL.Models.Customer>(ListOfCustomer);
                    }
                    IsLoadingData = false;
                }
            }
            catch (Exception ex)
            {
                Utils.Diagnostic.Log(ex);
            }
        }
        private double GetDistance(Location CurrentLocation, double Latitude, double Longtitude)
        {
            try
            {
                var result = Location.CalculateDistance(CurrentLocation.Latitude, CurrentLocation.Longitude, Latitude, Longtitude, DistanceUnits.Kilometers);
                return result;
            }
            catch (Exception ex)
            {
                Utils.Diagnostic.Log(ex);
                return 10 ^ 4;
            }
        }

        private Command bookedCommand;
        public ICommand BookedCommand => bookedCommand ??= new Command<BLL.Models.Customer>(async (customer) => await BookedOrder(customer).ConfigureAwait(false));

        private async Task BookedOrder(BLL.Models.Customer customer)
        {
            try
            {
                customer.BookedAtStr = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                customer.BookedBy = await Utils.LocalStorage.GetUserNameAsync(); ;
                await Utils.FirebaseDatabase.Instance.Client.Child($"Customers/{customer.ID}").PutAsync(JsonConvert.SerializeObject(customer));
                await LoadCustomer().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Utils.Diagnostic.Log(ex);
            }
        }

        private Command dialupCommand;
        public ICommand DialUpCommand => dialupCommand ??= new Command<BLL.Models.Customer>(async (customer) => await DialUp(customer).ConfigureAwait(false));

        private async Task DialUp(BLL.Models.Customer customer)
        {
            try
            {
                PhoneDialer.Open(customer.Phone);
            }
            catch (ArgumentNullException anEx)
            {
                await DisplayAlertAsync(localizer.Instance["ErrorOccured"], localizer.Instance["CanNotFoundPhoneNumber"], localizer.Instance["Okay"]).ConfigureAwait(false);
                Utils.Diagnostic.Log(anEx);
            }
            catch (FeatureNotSupportedException ex)
            {
                Utils.Diagnostic.Log(ex);
            }
            catch (Exception ex)
            {
                Utils.Diagnostic.Log(ex);
            }
        }
        private Command lunchMapCommand;
        public ICommand LunchMapCommand => lunchMapCommand ??= new Command<BLL.Models.Customer>((customer) => LunchMap(customer).ConfigureAwait(false));

        private async Task LunchMap(BLL.Models.Customer customer)
        {
            var location = new Location(customer.Latitude, customer.Longtitude);
            var options = new MapLaunchOptions { Name = $"منزل: {customer.Name}" };

            try
            {
                await Map.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync(localizer.Instance["ErrorOccured"], localizer.Instance["NoMapApplicationFound"], localizer.Instance["Okay"]).ConfigureAwait(false);
                Utils.Diagnostic.Log(ex);
            }
        }

        CancellationTokenSource cts;
        public async Task<Location> GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Utils.Diagnostic.Log(fnsEx, "Handle not supported on device exception");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Utils.Diagnostic.Log(fneEx, "Handle not enabled on device exception");
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Utils.Diagnostic.Log(pEx, "Handle permission exception");
            }
            catch (Exception ex)
            {
                // Unable to get location
                Utils.Diagnostic.Log(ex, "Unable to get location");
            }
            return await GetLastKnownLocation();
        }
        public async Task<Location> GetLastKnownLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Utils.Diagnostic.Log(fnsEx, "Handle not supported on device exception");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Utils.Diagnostic.Log(fneEx, "Handle not enabled on device exception");
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Utils.Diagnostic.Log(pEx, "Handle permission exception");
            }
            catch (Exception ex)
            {
                // Unable to get location
                Utils.Diagnostic.Log(ex, "Unable to get location");
            }
            return null;
        }

        ~MainViewModel()
        {
            MessagingCenter.Unsubscribe<App>(this, "UpdateStatusUser");
            Disconnect().ConfigureAwait(false);
        }
    }
}
