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
        private readonly CovidRepository _covidClient;
        public ICommand LoadCommand { get; set; }
        public ExtendedSplashViewModel()
        {
            _covidClient = new CovidRepository();
            LoadCommand = new AsyncCommand<object>(OnLoadCommandExecuteAsync);
        }

        private async Task OnLoadCommandExecuteAsync(object obj)
        {
            if (obj is Page page)
            {
                try
                {
                    Console.WriteLine("start fetching covid data");
                    await LoadDailyCountsAsync();
                    Console.WriteLine("end fetch covid data");
                    Console.WriteLine("covid data count" + App.Menuitems.Count);
                    Console.WriteLine("start fetching resource data");
                    LoadResource();
                    Console.WriteLine("end fetching resource data");
                    page.Frame.Navigate(typeof(MainPage));
                }
                catch (Exception ex)
                {
                    await new MessageDialog("Something went wrong" + ex.Message).ShowAsync();
                }
            }

        }
        private async Task LoadDailyCountsAsync()
        {
            if (App.Menuitems == null)
            {
                App.Menuitems = new List<StateWiseData>(await _covidClient.GetCovidCountsAsync());
            }
        }
        private void LoadResource()
        {
            if (App.AllResource == null)
            {
                var resource = _covidClient.GetResource();
                resource.ForEach(x => x.PhoneNumber = x.PhoneNumber.Replace("\n", ""));
                App.AllResource = new List<Resource>(resource);
            }
        }
    }
}
