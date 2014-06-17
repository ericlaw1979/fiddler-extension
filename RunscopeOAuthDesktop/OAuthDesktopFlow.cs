using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Fiddler;

namespace Runscope
{
    /// <summary>
    /// This class obtains the API token by navigating to the authorization page, then using Fiddler itself to watch
    /// for a request that contains the API token.
    /// </summary>
    public class OAuthDesktopFlow : IDisposable
    {
        private AutoResetEvent _Lock = new AutoResetEvent(false);
        private string _AuthCode;
        public string GetAuthCode(Uri authServer, Uri redirectUri)
        {
            ListenForRequest();
            Process.Start(authServer.OriginalString);
            _Lock.WaitOne(new TimeSpan(0,0,3,0));
            return _AuthCode;
        }

        /// <summary>
        /// Called on each Request Fiddler processes
        /// </summary>
        /// <param name="oS">The Fiddler Session object</param>
        void CheckRequest(Session oS)
        {
            if ((oS.port == 9696) && oS.HostnameIs("localhost"))  // MAGIC Value
            {
                string sQuery = Utilities.TrimBefore(oS.fullUrl, '?');
                if (!string.IsNullOrEmpty(sQuery) && sQuery.Contains("code"))
                {
                    var oNVC = Utilities.ParseQueryString(sQuery);
                    SetAuthCode(oNVC["code"]);
                    oS.utilCreateResponseAndBypassServer();
                    oS.utilSetResponseBody("<!doctype html><html>Runscope API key obtained. You may close this window.</html>");
                    oS.oResponse["Content-Type"] = "text/html";
                }
                else
                {
                    oS.utilCreateResponseAndBypassServer();
                    oS.responseCode = 404;
                    oS.utilSetResponseBody("<html>Not Found</html>");
                    oS.oResponse["Content-Type"] = "text/html";
                }
            }
        }

        public void ListenForRequest()
        {
            FiddlerApplication.BeforeRequest += CheckRequest;
        }

        public void StopListening()
        {
            FiddlerApplication.BeforeRequest -= CheckRequest;
        }

        public void SetAuthCode(string authCode)
        {
            _AuthCode = authCode;
            _Lock.Set();
        }

        public void Dispose()
        {
            StopListening();
        }

    }
}
