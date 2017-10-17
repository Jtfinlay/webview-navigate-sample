using System;
using Windows.UI.Xaml.Controls;
using Windows.Web;

namespace WebviewNavigateTest
{
    public class WebViewNavigationSuccessArgs
    {
        public bool IsSuccess { get; private set; }

        public Uri Uri { get; private set; }

        public WebErrorStatus WebErrorStatus { get; private set; }

        public WebViewNavigationSuccessArgs(WebViewNavigationCompletedEventArgs args)
        {
            IsSuccess = args.IsSuccess;
            Uri = args.Uri;
            WebErrorStatus = args.WebErrorStatus;
        }

        public WebViewNavigationSuccessArgs(bool isSuccess, Uri uri, WebErrorStatus webErrorStatus)
        {
            IsSuccess = isSuccess;
            Uri = uri;
            WebErrorStatus = webErrorStatus;
        }
    }
}
