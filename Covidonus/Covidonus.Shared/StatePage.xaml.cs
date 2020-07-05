using Covidonus.Shared.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Covidonus.Shared
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StatePage : Page
    {
        StateViewModel ViewModel;
        public StatePage()
        {
            this.InitializeComponent();
            ViewModel = new StateViewModel();
            this.DataContext = ViewModel;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (ViewModel != null)
                ViewModel.OnNavigatedTo(e);
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (ViewModel != null)
                ViewModel.OnNavigatingFrom(e);
        }
    }
}
