using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace WebviewNavigateTest
{
    public class WebviewNavigator
    {
        /// <summary>
        /// The maximum number of attempts to try a navigation.
        /// </summary>
        private const int MAX_RETRY_COUNT = 3;

        /// <summary>
        /// The maximum time allowed to perform a navigation.
        /// </summary>
        private const int MAX_TIMEOUT_MS = 1000;

        /// <summary>
        /// Access to the wrapped webview instance
        /// </summary>
        private readonly WebView _webView;

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private readonly DispatcherTimer _countdownTimer = new DispatcherTimer()
        {
            Interval = new TimeSpan(0, 0, 0, 0, MAX_TIMEOUT_MS)
        };

        /// <summary>
        /// Keep track of the desired navigation.
        /// </summary>
        private Action _navigationAction;

        /// <summary>
        /// Number of navigation attempts
        /// </summary>
        private int _navigationAttempt = 1;

        public event TypedEventHandler<WebView, WebViewNavigationSuccessArgs> NavigationCompleted;

        public WebviewNavigator(WebView webView)
        {
            _webView = webView;
            _webView.NavigationCompleted += OnNavigationCompleted;

            _countdownTimer.Tick += OnTimeoutElapsed;
        }

        public void Navigate(Uri source)
        {
            _navigationAction = () => _webView.Navigate(source);
            _navigationAttempt = 1;

            Navigate();
        }

        public void NavigateWithHttpRequestMessage(HttpRequestMessage requestMessage)
        {
            _navigationAction = () => _webView.NavigateWithHttpRequestMessage(requestMessage);
            _navigationAttempt = 1;

            Navigate();
        }

        private void OnTimeoutElapsed(object sender, object e)
        {
            _webView.Stop();

            _stopwatch.Stop();
            _countdownTimer.Stop();

            _navigationAttempt++;
            if (_navigationAttempt > MAX_RETRY_COUNT)
            {
                NavigationCompleted?.Invoke(_webView, new WebViewNavigationSuccessArgs(false, null, Windows.Web.WebErrorStatus.Timeout));
                return;
            }

            Navigate();
        }

        private void Navigate()
        {
            _stopwatch.Restart();
            _countdownTimer.Start();

            _navigationAction.Invoke();
        }

        private void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            _stopwatch.Stop();
            _countdownTimer.Stop();

            if (args.IsSuccess)
            {
                NavigationCompleted?.Invoke(sender, new WebViewNavigationSuccessArgs(args));
                return;
            }

            _navigationAttempt++;
            if (_navigationAttempt > MAX_RETRY_COUNT)
            {
                NavigationCompleted?.Invoke(sender, new WebViewNavigationSuccessArgs(args));
                return;
            }

            Navigate();
        }
    }
}
