using BasicMvvm;
using BasicMvvm.Commands;
using Covidonus.Data.Models;
using Covidonus.Data.Repositories;

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
        //private static CovidClient _covidClient;

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
                    await LoadDailyCountsAsync(); await LoadResourceAsync();
                    page.Frame.Navigate(typeof(MainPage));
                }
                catch (Exception ex)
                {
                    await new MessageDialog("Something went wrong" + ex.Message).ShowAsync();
                }
            }

        }
        private static async Task LoadDailyCountsAsync()
        {
            if (App.Menuitems == null)
            {
                await new SeedDataRepository().RefreshCovidDataAsync();
                // if (_covidClient == null)
                //_ovidClient = new CovidClient();
                //await new MessageDialog("Start fetch" + _covidClient.BaseUrl).ShowAsync();
                //var res = await _covidClient.GetCovidCountsAsync();
                //await new MessageDialog("End fetch").ShowAsync();
                App.Menuitems = new List<StateWiseData>(SeedDataRepository.MenuList);
            }
        }
        private async Task LoadResourceAsync()
        {
            if (App.AllResource == null)
            {

                var resource = SeedDataRepository.ResourceList;
                resource.ForEach(x => x.PhoneNumber = x.PhoneNumber.Replace("\n", ""));
                App.AllResource = new List<Resource>(resource);
            }
        }
    }
}
