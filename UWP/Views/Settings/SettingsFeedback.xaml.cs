using Microsoft.AppCenter.Analytics;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Email;
using Windows.Services.Store;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWP.Views {
    public sealed partial class SettingsFeedback : Page {

        private PackageVersion version;

        public SettingsFeedback() {
            InitializeComponent();

            version = Package.Current.Id.Version;
        }

        private async void Feedback_Rating(object sender, RoutedEventArgs e) {
            Analytics.TrackEvent("feedback-rating");
            await ShowRatingReviewDialog();
        }
        private async void Feedback_Review(object sender, RoutedEventArgs e) {
            Analytics.TrackEvent("feedback-review");
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=9n3b47hbvblc"));
        }
        private async void Feedback_Mail(object sender, RoutedEventArgs e) {
            Analytics.TrackEvent("feedback-mail");
            EmailMessage emailMessage = new EmailMessage();
            emailMessage.To.Add(new EmailRecipient("ismael.em@outlook.com"));
            emailMessage.Subject = "Feedback for CryptoTracker v" + string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
            emailMessage.Body = "<Insert here suggestions/bugs/feature requests...> \n\n";

            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }
        private async void TwitterButton_Click(object sender, RoutedEventArgs e) {
            Analytics.TrackEvent("feedback-twitter");
            await Launcher.LaunchUriAsync(new Uri("https://twitter.com/ismaelestalayo"));
        }

        private async void GithubRepo_Click(object sender, RoutedEventArgs e) {
            Analytics.TrackEvent("feedback-githubRepo");
            await Launcher.LaunchUriAsync(new Uri("https://github.com/ismaelestalayo/CryptoTracker"));
        }

        /// #######################################################################################
        private async void Donation_Paypal(object sender, RoutedEventArgs e) {
            Analytics.TrackEvent("feedback-donationPaypal");
            await Launcher.LaunchUriAsync(new Uri("https://paypal.me/ismaelEstalayo"));
            await LottiePlayer.PlayAsync(0, 1, false);
        }

        private async void Donation_Kofi(object sender, RoutedEventArgs e) {
            Analytics.TrackEvent("feedback-donationKofi");
            await Launcher.LaunchUriAsync(new Uri("https://ko-fi.com/ismaelestalayo"));
            await LottiePlayer.PlayAsync(0, 1, false);
        }

        private void Donation_Crypto(object sender, RoutedEventArgs e) {
            Analytics.TrackEvent("feedback-donationCrypto");
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText("0xE1D40ce30E257af4753e607f7F4a4feC8900E3cD");
        }

        /// #######################################################################################
        public async Task<bool> ShowRatingReviewDialog() {
            StoreContext _storeContext = StoreContext.GetDefault(); ;
            StoreRateAndReviewResult result = await _storeContext.RequestRateAndReviewAppAsync();

            switch (result.Status) {
                case StoreRateAndReviewStatus.Succeeded:
                    await LottiePlayer.PlayAsync(0, 1, false);
                    break;
                case StoreRateAndReviewStatus.CanceledByUser:
                    break;
                case StoreRateAndReviewStatus.Error:
                case StoreRateAndReviewStatus.NetworkError:
                    await new MessageDialog("Something went wrong.").ShowAsync();
                    break;
                default:
                    break;
            }
            // There was an error with the request, or the customer chose not to
            // rate or review the app.
            return false;
        }
    }
}
