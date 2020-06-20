using Covidonus.Shared.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Uno.Extensions.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Covidonus
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
#if WINDOWS_UWP
			NavView.IsSettingsVisible = true;
			// Change the settings text to toggle the theme
			var settingsItem = (NavigationViewItem)NavView.SettingsItem;
			settingsItem.Content = "Toggle Light/Dark theme";
#else
            NavView.IsSettingsVisible = false;
#endif
            _ = InitializeNavigationViewItemsAsync();

        }

        private async Task InitializeNavigationViewItemsAsync()
        {
            StateClient stateClient = new StateClient();
            var menu = await stateClient.GetAsync();
            menu.ToList().ForEach(m =>
            {
                NavView.MenuItems.Add(new NavigationViewItem()
                {
                    Content = m.State,
                    Icon = new SymbolIcon(Symbol.Next),
                    Tag = m.StateCode,
                });
            });
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        { }


    }
}
