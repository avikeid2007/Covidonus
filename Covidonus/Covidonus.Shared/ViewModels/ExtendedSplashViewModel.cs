using BasicMvvm;
using BasicMvvm.Commands;
using Covidonus.Swag;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Covidonus.Shared.ViewModels
{
    public class ExtendedSplashViewModel : BindableBase
    {
        private static CovidClient _covidClient;

        public ICommand LoadCommand { get; set; }
        public ExtendedSplashViewModel()
        {
            LoadCommand = new AsyncCommand<object>(OnLoadCommandExecuteAsync);
        }

        private async Task OnLoadCommandExecuteAsync(object obj)
        {
            if (obj is Page page)
            {
                await LoadDailyCountsAsync();
                page.Frame.Navigate(typeof(MainPage));
            }

        }
        private static async Task LoadDailyCountsAsync()
        {
            try
            {
                _covidClient = new CovidClient();
                var res = await _covidClient.GetCovidCountsAsync();
                App.Menuitems = new List<StateWiseData>(res);
            }
            catch (Exception ex)
            { }
        }
    }
}
