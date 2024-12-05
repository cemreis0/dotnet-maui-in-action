namespace FindMe
{
    public partial class MainPage : ContentPage
    {
        string _locationURL = "https://bing.com/maps/default.aspx?cp=";

        public string Username { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnFindMeClicked(object sender, EventArgs e)
        {
            var permissions = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            
            if (permissions == PermissionStatus.Granted)
            {
                await ShareLocation();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(
                    "Permission Error",
                    "You have not granted the app permission to access your location.",
                    "OK"
                    );

                var requested = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (requested == PermissionStatus.Granted)
                {
                    await ShareLocation();
                }
                else
                {
                    if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst)
                    {
                        await App.Current.MainPage.DisplayAlert(
                            "Permission Error",
                            "Location is required to share it. Please enable location for this app in Settings.",
                            "OK"
                            );
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(
                            "Permission Error",
                            "Location is required to share it. We'll ask again next time.",
                            "OK"
                            );
                    }
                }
            }
        }

        private async Task ShareLocation()
        {
            Username = UsernameEntry.Text;

            var locationRequest = new GeolocationRequest(GeolocationAccuracy.Best);

            var location = await Geolocation.GetLocationAsync(locationRequest);

            await Share.RequestAsync(new ShareTextRequest
            {
                Subject = "Find me!",
                Title = "Find me!",
                Text = $"{Username} is sharing their location with you",
                Uri = $"{_locationURL}{location.Latitude}~{location.Longitude}"
            });
        }
    }

}
