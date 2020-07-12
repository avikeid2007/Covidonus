using BasicMvvm;
using BasicMvvm.Commands;
using Covidonus.Shared.Helpers;
using Covidonus.Swag;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Xamarin.Essentials;

namespace Covidonus.Shared.ViewModels
{
    public class StateViewModel : BindableBase
    {
        private Visibility _isVisibleindiaCounts;
        private Visibility _isVisibleStateCounts;
        private Visibility _isFabStateVisible;
        private StateWiseData _selectedstate;
        private IList _stateCollection;
        private string _newCases;
        private string _newDeaths;
        private bool _isFavoriteState;
        private Visibility _isResourceVisible;
        private ObservableCollection<Swag.Resource> _stateResources;
        private Visibility _isCovidCountVisible;
        private List<string> _resourceCategoryList;
        private string _selectedResourceCategory;
        public ICommand FavoriteCommand { get; set; }
        public ICommand ResourceCommand { get; set; }
        public ICommand ImportantCommand { get; set; }
        public ICommand NotificationCommand { get; set; }
        public ICommand ShareCommand { get; set; }
        public ICommand DialerCommand { get; set; }
        public StateViewModel()
        {
            IsVisibleIndiaCounts = Visibility.Collapsed;
            IsVisibleStateCounts = Visibility.Collapsed;
            FavoriteCommand = new DelegateCommand(OnFavoriteCommandExecute);
            ShareCommand = new AsyncCommand(OnShareCommandExecuteAsync);
            ResourceCommand = new DelegateCommand(OnResourceCommandExecute);
            DialerCommand = new DelegateCommand<string>(OnDialerCommandExecute);
            IsResourceVisible = Visibility.Collapsed;
            IsCovidCountVisible = Visibility.Visible;
        }

        private void OnDialerCommandExecute(string numbers)
        {
            if (numbers.Contains(','))
                numbers = numbers.Split(',').FirstOrDefault();
            PhoneDialer.Open(numbers);
        }
        public List<string> ResourceCategoryList
        {
            get { return _resourceCategoryList; }
            set
            {
                _resourceCategoryList = value;
                OnPropertyChanged();
            }
        }
        public string SelectedResourceCategory
        {
            get { return _selectedResourceCategory; }
            set
            {
                _selectedResourceCategory = value;
                if (!string.IsNullOrEmpty(value))
                {
                    if (value == "All")
                    {
                        StateResources = new ObservableCollection<Swag.Resource>(App.AllResource.Where(x => x.State == SelectedState.State));
                    }
                    else
                    {
                        StateResources = new ObservableCollection<Swag.Resource>(App.AllResource.Where(x => x.State == SelectedState.State && x.Category == value));
                    }
                }
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Swag.Resource> StateResources
        {
            get { return _stateResources; }
            set
            {
                _stateResources = value;
                OnPropertyChanged();
            }
        }
        private void OnResourceCommandExecute()
        {
            if (IsResourceVisible == Visibility.Visible)
            {
                IsResourceVisible = Visibility.Collapsed;
                IsCovidCountVisible = Visibility.Visible;
            }
            else
            {
                IsResourceVisible = Visibility.Visible;
                IsCovidCountVisible = Visibility.Collapsed;
            }

            var Categories = new List<string>(App.AllResource.Where(x => x.State == SelectedState.State).Select(x => x.Category).Distinct())
            {
                "All"
            };
            ResourceCategoryList = new List<string>(Categories.OrderBy(x => x));
            SelectedResourceCategory = "All";
        }

        private async Task OnShareCommandExecuteAsync()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = SelectedState.Confirmed.ToString(),
                Title = SelectedState.State
            });
        }

        private void OnFavoriteCommandExecute()
        {
            if (IsFavoriteState)
                LocalSettingsHelper.MarkContainer(SettingContainer.Favorite, "StateCode", string.Empty);
            else
                LocalSettingsHelper.MarkContainer(SettingContainer.Favorite, "StateCode", SelectedState.StateCode);
            SetFavoriteIcon();
        }
        public bool IsFavoriteState
        {
            get { return _isFavoriteState; }
            set
            {
                _isFavoriteState = value;
                OnPropertyChanged();
            }
        }
        public string NewCases
        {
            get { return _newCases; }
            set
            {
                _newCases = value;
                OnPropertyChanged();
            }
        }
        public string NewDeaths
        {
            get { return _newDeaths; }
            set
            {
                _newDeaths = value;
                OnPropertyChanged();
            }
        }
        public IList StateCollection
        {
            get { return _stateCollection; }
            set
            {
                _stateCollection = value;
                OnPropertyChanged();
            }

        }
        public StateWiseData SelectedState
        {
            get { return _selectedstate; }
            set
            {
                _selectedstate = value;
                OnPropertyChanged();
            }
        }
        public Visibility IsVisibleIndiaCounts
        {
            get { return _isVisibleindiaCounts; }
            set
            {
                _isVisibleindiaCounts = value;
                OnPropertyChanged();
            }
        }
        public Visibility IsVisibleStateCounts
        {
            get { return _isVisibleStateCounts; }
            set
            {
                _isVisibleStateCounts = value;
                OnPropertyChanged();
            }
        }
        public Visibility IsFabStateVisible
        {
            get { return _isFabStateVisible; }
            set
            {
                _isFabStateVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility IsResourceVisible
        {
            get { return _isResourceVisible; }
            set
            {
                _isResourceVisible = value;
                OnPropertyChanged();
            }
        }
        public Visibility IsCovidCountVisible
        {
            get { return _isCovidCountVisible; }
            set
            {
                _isCovidCountVisible = value;
                OnPropertyChanged();
            }
        }
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string stateCode && App.Menuitems?.Count > 0)
            {
                if (stateCode.Equals("TT", System.StringComparison.OrdinalIgnoreCase))
                {
                    IsVisibleIndiaCounts = Visibility.Visible;
                    IsVisibleStateCounts = Visibility.Collapsed;
                    IsFabStateVisible = Visibility.Collapsed;

                    StateCollection = App.Menuitems.OrderByDescending(x => x.Confirmed).Skip(1).Select(x => new
                    {

                        District = x.StateCode == "DN" ? "Daman & Diu" : x.StateCode == "AN" ? "Andaman & Nicobar" : x.State,
                        Confirmed = x.Confirmed,
                        Active = x.Active,
                        Recovered = x.Recovered,
                        Deaths = x.Deaths,
                    }).ToList();
                    SelectedState = App.Menuitems.FirstOrDefault(x => x.StateCode.Equals(stateCode, System.StringComparison.OrdinalIgnoreCase));
                    SetNewCounts(SelectedState);
                }
                else
                {
                    IsVisibleIndiaCounts = Visibility.Collapsed;
                    IsVisibleStateCounts = Visibility.Visible;
                    IsFabStateVisible = Visibility.Visible;
                    SelectedState = App.Menuitems.FirstOrDefault(x => x.StateCode.Equals(stateCode, System.StringComparison.OrdinalIgnoreCase));
                    StateCollection = SelectedState.DistrictData.Select(x => new
                    {
                        District = x.District,
                        Confirmed = x.Confirmed,
                        Active = x.Active,
                        Recovered = x.Recovered,
                        Deaths = x.Deceased,
                    }).ToList();
                    SetNewCounts(App.Menuitems.FirstOrDefault(x => x.StateCode.Equals("TT", System.StringComparison.OrdinalIgnoreCase)));
                }
                SetFavoriteIcon();
                IsResourceVisible = Visibility.Collapsed;
                IsCovidCountVisible = Visibility.Visible;
            }

        }

        private void SetNewCounts(StateWiseData indiaData)
        {
            if (indiaData != null)
            {
                NewCases = $"+{indiaData.TodayConfirmed}";
                NewDeaths = $"+{indiaData.TodayDeaths}";
            }
        }
        private void SetFavoriteIcon()
        {
            var code = GetFavoriteState();
            IsFavoriteState = !string.IsNullOrEmpty(code) && code == SelectedState?.StateCode;
        }
        private string GetFavoriteState()
        {
            try
            {
                return LocalSettingsHelper.GetContainerValue<string>(SettingContainer.Favorite, "StateCode");
            }
            catch
            {
                return string.Empty;
            }
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {

        }
    }
}
