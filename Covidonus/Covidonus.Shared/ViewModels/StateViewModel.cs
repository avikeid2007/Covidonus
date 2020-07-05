using BasicMvvm;
using Covidonus.Swag;
using System.Collections;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

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

        public StateViewModel()
        {
            IsVisibleIndiaCounts = Visibility.Collapsed;
            IsVisibleStateCounts = Visibility.Collapsed;
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
                        District = x.State,
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

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {

        }
    }
}
