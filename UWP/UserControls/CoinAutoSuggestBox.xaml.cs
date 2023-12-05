using System;
using System.Collections.Generic;
using System.Linq;
using UWP.Helpers;
using UWP.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWP.UserControls {
    public sealed partial class CoinAutoSuggestBox : UserControl {
        public CoinAutoSuggestBox() {
            InitializeComponent();
        }

        public static readonly DependencyProperty CoinProperty =
            DependencyProperty.Register(nameof(Coin), typeof(SuggestionCoin), typeof(SuggestionCoin), null);

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(string), null);


        public SuggestionCoin Coin {
            get => (SuggestionCoin)GetValue(CoinProperty);
            set {
                SetValue(CoinProperty, value);
                if (value?.Symbol != "NULL" && value != null)
                    AutoSuggestBox.Text = value.Symbol;
            }
        }
        public string Header {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// #######################################################################################
        ///  AutoSuggestBox events
        private void AutoSuggestBox_GotFocus(object sender, RoutedEventArgs e) {
            AutoSuggestBox box = sender as AutoSuggestBox;
            box.ItemsSource = FilterCoins(box);
        }

        /// When tabbing out of the SuggestBox sometimes the text gets cleared out
        private void AutoSuggestBox_LostFocus(object sender, RoutedEventArgs e) {
            var searched = AutoSuggestBox.Text.ToUpperInvariant();
            var matches = App.coinListPaprika.Where(x => x.symbol == searched).ToList();

            if (searched == "" && Coin.Symbol != "NULL")
                AutoSuggestBox.Text = Coin.Symbol;
            else if (matches.Count() > 0)
                Coin = new SuggestionCoin(matches[0].symbol, matches[0].name);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
            // Only get results when it was a user typing, 
            // otherwise assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                sender.ItemsSource = FilterCoins(sender);
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
            => AutoSuggestBox.Text = ((SuggestionCoin)args.SelectedItem).Symbol;

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
            => Coin = (SuggestionCoin)args.ChosenSuggestion;

        /// #######################################################################################
        ///  Filter (also used in the MainPage)
        public static List<SuggestionCoin> FilterCoins(AutoSuggestBox box) {
            var filtered = App.coinListPaprika.Where(x =>
                x.symbol.Contains(box.Text, StringComparison.InvariantCultureIgnoreCase) ||
                x.name.Contains(box.Text, StringComparison.InvariantCultureIgnoreCase)).ToList();
            List<SuggestionCoin> list = new List<SuggestionCoin>();
            foreach (var coin in filtered) {
                list.Add(new SuggestionCoin {
                    Icon = IconsHelper.GetIcon(coin.symbol),
                    Name = coin.name,
                    Symbol = coin.symbol
                });
            }
            return list;
        }
    }
}
