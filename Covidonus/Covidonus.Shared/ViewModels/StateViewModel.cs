using BasicMvvm;
using Covidonus.Swag;
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
        private DailyTotalCount _dailyCounts;
        private StateData _selectedstate;
        public StateViewModel()
        {
            IsVisibleIndiaCounts = Visibility.Collapsed;
            IsVisibleStateCounts = Visibility.Collapsed;
            DailyCounts = App.DailyCounts;
        }
        public StateData SelectedState
        {
            get { return _selectedstate; }
            set
            {
                _selectedstate = value;
                OnPropertyChanged();
            }
        }
        public DailyTotalCount DailyCounts
        {
            get { return _dailyCounts; }
            set
            {
                _dailyCounts = value;
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
            if (e.Parameter is string stateCode)
            {
                if (App.Menuitems?.Count > 0 && App.DailyCounts != null)
                {
                    if (DailyCounts == null)
                    {
                        DailyCounts = App.DailyCounts;
                    }
                    if (stateCode.Equals("TT", System.StringComparison.OrdinalIgnoreCase))
                    {
                        IsVisibleIndiaCounts = Visibility.Visible;
                        IsVisibleStateCounts = Visibility.Collapsed;
                        IsFabStateVisible = Visibility.Collapsed;
                    }
                    else
                    {
                        IsVisibleIndiaCounts = Visibility.Collapsed;
                        IsVisibleStateCounts = Visibility.Visible;
                        IsFabStateVisible = Visibility.Visible;
                    }
                    SelectedState = App.Menuitems.FirstOrDefault(x => x.StateCode.Equals(stateCode, System.StringComparison.OrdinalIgnoreCase));
                    //App.Menuitems.FirstOrDefault(x => x.StateCode == stateCode)
                }
            }

        }
        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {

        }
    }
}
