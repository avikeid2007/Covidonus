using BasicMvvm;
using BasicMvvm.Commands;
using Covidonus.Shared.Helpers;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Covidonus.Shared.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private Frame _contentFrame;

        public ICommand LoadCommand { get; set; }
        public ICommand ItemInvokedCommand { get; set; }
        public MainViewModel()
        {
            LoadCommand = new DelegateCommand<object>(OnLoadCommandExecute);
            ItemInvokedCommand = new DelegateCommand<NavigationViewItemInvokedEventArgs>(OnItemInvokedCommandExecute);
        }
        private void OnItemInvokedCommandExecute(NavigationViewItemInvokedEventArgs obj)
        {
            if (_contentFrame != null)
            {
                if (obj.IsSettingsInvoked)
                {
                    ToggleTheme();
                }
                else if (obj.InvokedItemContainer is NavigationViewItem item)
                {
                    _contentFrame.Navigate(typeof(StatePage), Convert.ToString(item.Tag));
                }
            }
        }

        private void ToggleTheme()
        {
#if WINDOWS_UWP
            // Set theme for window root.
            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                if (frameworkElement.ActualTheme == ElementTheme.Dark)
                {
                    frameworkElement.RequestedTheme = ElementTheme.Light;
                }
                else
                {
                    frameworkElement.RequestedTheme = ElementTheme.Dark;
                }
            }
#endif
        }

        private void OnLoadCommandExecute(object obj)
        {
            _contentFrame = obj as Frame;
            var code = GetFavoriteState();
            if (!string.IsNullOrEmpty(code))
            {
                _contentFrame.Navigate(typeof(StatePage), code);
            }
            else
            {
                _contentFrame.Navigate(typeof(StatePage), "TT");
            }
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
    }
}
