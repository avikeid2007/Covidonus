using BasicMvvm;
using BasicMvvm.Commands;
using Covidonus.Swag;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Uno.Extensions;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Covidonus.Shared.ViewModels
{
    public class ExtendedSplashViewModel : BindableBase
    {
        private static IClient _covidClient;

        public ICommand LoadCommand { get; set; }
        public ExtendedSplashViewModel()
        {
            LoadCommand = new AsyncCommand<object>(OnLoadCommandExecuteAsync);
        }

        private async Task OnLoadCommandExecuteAsync(object obj)
        {
            if (obj is Page page)
            {
                try
                {
                    await Task.WhenAll(LoadDailyCountsAsync(), LoadResourceAsync());
                    page.Frame.Navigate(typeof(MainPage));
                }
                catch
                {
                    await new MessageDialog("Something went wrong").ShowAsync();
                }
            }

        }
        private static async Task LoadDailyCountsAsync()
        {
            if (App.Menuitems == null)
            {
                // if (_covidClient == null)
                _covidClient = new Client(CovidBaseClient.DefaultBaseUrl);
                var res = await _covidClient.CountsAsync();
                App.Menuitems = new List<StateWiseData>(res);
            }
        }
        private async Task LoadResourceAsync()
        {
            if (App.AllResource == null)
            {
                if (_covidClient == null)
                    _covidClient = new Client(CovidBaseClient.DefaultBaseUrl);
                var resource = await _covidClient.ResourceAsync();
                resource.ForEach(x => x.PhoneNumber = x.PhoneNumber.Replace("\n", ""));
                App.AllResource = new List<Resource>(resource);
            }
        }
    }
}
