using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;
using Runscope;

namespace RunscopeFiddlerExtension
{
    public class ShareRequest : IFiddlerExtension
    {
        MenuItem _SendMenuItem = new MenuItem("Send request to Runscope");
        MenuItem _ConfigureRunscope = new MenuItem("Configure Runscope");

        private string _Bucket;
        private string _ApiKey;
        private bool _UseProxy;
        private HttpClient _Client;

        public void OnLoad()
        {
            _SendMenuItem.Click += (s, e) => ShareSelectedRequests();
            _ConfigureRunscope.Click += (s, e) => ConfigureRunscope();
    
            FiddlerApplication.UI.lvSessions.ContextMenu.MenuItems.Add(_SendMenuItem);
            FiddlerApplication.UI.mnuTools.MenuItems.Add(_ConfigureRunscope);

            InitSettings();


            _Client = CreateHttpClient();
            UpdateApiKey();
        
        }

        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler { UseProxy = _UseProxy };
            return new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.runscope.com")
            };
        }

        private void ConfigureRunscope()
        {
            var form = new ConfigureForm();
            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Update settings
            }

        }

        private void InitSettings()
        {
            _UseProxy = FiddlerApplication.Prefs.GetBoolPref("runscope.useproxy", false);
            _ApiKey = FiddlerApplication.Prefs.GetStringPref("runscope.apikey", "missingapikey");
            if (_ApiKey == "missingapikey" || String.IsNullOrWhiteSpace(_ApiKey))
            {

                FiddlerApplication.Prefs.SetStringPref("runscope.apikey", "missingapikey");
            }


            _Bucket = FiddlerApplication.Prefs.GetStringPref("runscope.bucketkey", "missingbucketkey");
            if (_Bucket == "missingbucketkey" || String.IsNullOrWhiteSpace(_Bucket))
            {

                FiddlerApplication.Prefs.SetStringPref("runscope.bucketkey", "missingbucketkey");
            }
            FiddlerApplication.Prefs.AddWatcher("runscope.", OnPrefChange);
        }

        private void UpdateApiKey()
        {
            _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _ApiKey);
                // "d93bf241-3653-464b-bddd-4b3dad60894f"
        }

        private void OnPrefChange(object sender, PrefChangeEventArgs oPref)
        {
            if (oPref.PrefName == "runscope.apikey")
            {
                _ApiKey = oPref.ValueString;
                UpdateApiKey();
            }
            if (oPref.PrefName == "runscope.bucketkey")
            {
                _Bucket = oPref.ValueString;
            }
            if (oPref.PrefName == "runscope.useproxy")
            {
                _UseProxy = oPref.ValueBool;
                _Client = CreateHttpClient();
            }
        }

        public HttpClient Client
        {
            get { return _Client ?? (_Client = CreateHttpClient()); }
        }

        private bool ConfigComplete()
        {
            return (!String.IsNullOrEmpty(_ApiKey) && !_ApiKey.StartsWith("missing") &&
                    !String.IsNullOrEmpty(_Bucket) &&
                    !_Bucket.StartsWith("missing"));
        }

        public void OnBeforeUnload()
        {
            _SendMenuItem.Dispose();
            _ConfigureRunscope.Dispose();
            _Client.Dispose();
        }

        private void ShareSelectedRequests()
        {
            if (!ConfigComplete())
            {
                var accessToken = GetAuthToken();
                
                _ApiKey = accessToken;
                FiddlerApplication.Prefs.SetStringPref("runscope.apikey", _ApiKey);
                UpdateApiKey();
            }

            var task = SendRequests();
           // task.Wait();

            FiddlerApplication.UI.SetStatusText("Completed sending requests to Runscope");

        }

        public string GetAuthToken()
        {
            var redirectUri = "http://localhost:9696/";
            var oauthLink = new OAuth2AuthorizeLink();
            string authcode;
            using (var flow = new OAuthDesktopFlow())
            {
                authcode = flow.GetAuthCode(oauthLink.Target, new Uri(redirectUri));
                Thread.Sleep(3000);  // Let web browser render page
            }

            var tokenLink = new OAuth2TokenLink();
            
            var request = tokenLink.BuildRequest(new Dictionary<string, string>()
            {
                {"client_id", "1acc6a2e-dd01-49ad-a856-e24789c77529"},
                {"client_secret", "42ebcddd-6988-4835-895c-cff19bdbdbe9"},
                {"code", authcode},
                {"grant_type", "authorization_code"},
                {"redirect_uri", redirectUri}
            });

            var response = Client.SendAsync(request).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var token = OAuth2TokenLink.ParseTokenBody(response.Content.ReadAsStringAsync().Result);
                return token.AccessToken;
            }
            else
            {
                var error = OAuth2TokenLink.ParseErrorBody(response.Content.ReadAsStringAsync().Result);
                throw new Exception("Failed to obtain token : " + error.ErrorDescription);
            }
         
        }

        private async Task SendRequests()
        {
            var messagesLink = new MessagesLink();
            var list = FiddlerApplication.UI.GetSelectedSessions();

            foreach (Session session in list)
            {
                var srequest = CreateRequestFromSession(session);
                var sresponse = CreateResponseFromSession(session);
                var request = messagesLink.BuildPOSTRequest(_Bucket, srequest,sresponse); //es3pfvznehtn
                var response = await _Client.SendAsync(request);
                
            }

        }

        private HttpRequestMessage CreateRequestFromSession(Session session)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(session.url, UriKind.RelativeOrAbsolute),
                Method = new HttpMethod(session.RequestMethod)
            };
            var failedHeaders = new List<HTTPHeaderItem>();
            foreach (var header in session.oRequest.headers)
            {
                if (!request.Headers.TryAddWithoutValidation(header.Name, header.Value))
                {
                    failedHeaders.Add(header);
                }
            }
            if (session.RequestBody.Length > 0)
            {
                request.Content = new ByteArrayContent(session.RequestBody);
                foreach (var header in failedHeaders)
                {
                    request.Content.Headers.TryAddWithoutValidation(header.Name, header.Value);
                }
            }
            return request;
        }

        private HttpResponseMessage CreateResponseFromSession(Session session)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = (HttpStatusCode) session.responseCode
            };
            var failedHeaders = new List<HTTPHeaderItem>();
            foreach (var header in session.oResponse.headers)
            {
                if(!response.Headers.TryAddWithoutValidation(header.Name, header.Value))
                {
                    failedHeaders.Add(header);
                }
            }

            if (session.ResponseBody.Length > 0)
            {
                response.Content = new ByteArrayContent(session.ResponseBody);
                foreach (var header in failedHeaders)
                {
                    response.Content.Headers.TryAddWithoutValidation(header.Name, header.Value);
                }
            }
            return response;
        }

    }

   
}
