using System;
using Windows.UI.Xaml.Controls;

namespace WebviewNavigateTest
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var navigator = new WebviewNavigator(_webView);
            navigator.Navigate(new Uri("https://jtfinlay.github.io/"));
        }
    }
}
