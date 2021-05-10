using CovidonusV2.Swag;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uno.Extensions;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CovidonusV2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplashPage : Page
    {
        public ExtendedSplashPage()
        {
            this.InitializeComponent();
        }

        private async void splashPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                await new MessageDialog("Load data start").ShowAsync();
                await LoadDailyCountsAsync();
                await LoadResourceAsync();
                await new MessageDialog("Load data Completed").ShowAsync();
                Frame.Navigate(typeof(MainPage));
            }
            catch (Exception ex)
            { await new MessageDialog(ex.Message).ShowAsync(); }
        }

        private async Task LoadDailyCountsAsync()
        {
            if (App.Menuitems == null)
            {
                //if (_covidClient == null)
                var _covidClient = new Swag.Client("http://covidonus.avnishkumar.co.in/");
                var res = await _covidClient.CountsAsync();
                App.Menuitems = new List<StateWiseData>(res);
            }
        }
        private async Task LoadResourceAsync()
        {
            if (App.AllResource == null)
            {
                //if (_covidClient == null)
                var _covidClient = new Swag.Client("http://covidonus.avnishkumar.co.in/");
                var resource = await _covidClient.ResourceAsync();
                resource.ForEach(x => x.PhoneNumber = x.PhoneNumber.Replace("\n", ""));
                App.AllResource = new List<Resource>(resource);
            }
        }
    }
}
