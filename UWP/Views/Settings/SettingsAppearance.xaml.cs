using Microsoft.UI.Xaml.Controls;
using UWP.Core.Constants;
using UWP.Shared.Constants;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWP.Views {
    public sealed partial class SettingsAppearance : Page {

        /// To initialize toggle switches
        internal bool InitialBackBtnStatus { get; } = App._LocalSettings.Get<bool>(UserSettings.IsBackButtonVisible);
        internal bool InitialMinimalStatus { get; } = App._LocalSettings.Get<bool>(UserSettings.Minimal);
        internal bool InitialMonochromeStatus { get; } = App._LocalSettings.Get<bool>(UserSettings.Monochrome);
        internal bool InitialBWStatus { get; } = App._LocalSettings.Get<bool>(UserSettings.BW);

        public SettingsAppearance() {
            InitializeComponent();
            Loaded += SettingsAppearance_Loaded;

        }

        private void SettingsAppearance_Loaded(object sender, RoutedEventArgs e) {
            var theme = App._LocalSettings.Get<string>(UserSettings.Theme);
            switch (theme) {
                case "Light":
                    ThemeRadioButtons.SelectedIndex = 0;
                    break;
                case "Dark":
                    ThemeRadioButtons.SelectedIndex = 1;
                    break;
                default:
                case "Windows":
                    ThemeRadioButtons.SelectedIndex = 2;
                    break;
            }
        }


        // ###############################################################################################
        private void ThemeRadioButtons_Changed(object sender, SelectionChangedEventArgs e) {
            RadioButtons r = sender as RadioButtons;
            if (r.SelectedIndex < 0)
                return;
            var theme = ((ContentControl)r.SelectedItem).Content.ToString();

            var parentFrame = (Frame)Window.Current.Content;
            var parentDialog = (FrameworkElement)((FrameworkElement)((FrameworkElement)this.Parent).Parent).Parent;

            App._LocalSettings.Set(UserSettings.Theme, theme);
            var darkTheme = ColorConstants.CurrentThemeIsDark();
            parentFrame.RequestedTheme = darkTheme ? ElementTheme.Dark : ElementTheme.Light;
            parentDialog.RequestedTheme = darkTheme ? ElementTheme.Dark : ElementTheme.Light;
        }

        private void BackButtonSwitch_Toggled(object sender, RoutedEventArgs e)
            => App._LocalSettings.Set(UserSettings.IsBackButtonVisible, ((ToggleSwitch)sender).IsOn);

        private void MinimalSwitch_Toggled(object sender, RoutedEventArgs e)
            => App._LocalSettings.Set(UserSettings.Minimal, ((ToggleSwitch)sender).IsOn);

        private void MonochromeSwitch_Toggled(object sender, RoutedEventArgs e)
            => App._LocalSettings.Set(UserSettings.Monochrome, ((ToggleSwitch)sender).IsOn);

        private void BWSwitch_Toggled(object sender, RoutedEventArgs e)
            => App._LocalSettings.Set(UserSettings.BW, ((ToggleSwitch)sender).IsOn);

    }
}
