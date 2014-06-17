using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;
using Runscope;

namespace RunscopeFiddlerExtension
{
    public class ShareRequest : IFiddlerExtension
    {
        private readonly IFiddlerHostAdapter _hostAdapter;
        MenuItem _SendMenuItem = new MenuItem("Share request on Runscope");
        MenuItem _ConfigureRunscope = new MenuItem("Configure Runscope");

        private RunscopeSettings _runscopeSettings;
        private HttpClient _Client;

        public ShareRequest() : this(null)
        {
            
        }
        public ShareRequest(IFiddlerHostAdapter hostAdapter)
        {
            _hostAdapter = hostAdapter ?? new FiddlerHostAdapater();
        }

        public void OnLoad()
        {
            _SendMenuItem.Click += (s, e) => ShareSelectedRequests();
            _ConfigureRunscope.Click += (s, e) => ConfigureRunscope();

            _hostAdapter.InstallConfigMenu(_ConfigureRunscope);
            _hostAdapter.InstallContextMenu(_SendMenuItem);

            _runscopeSettings = new RunscopeSettings(_hostAdapter.Preferences);

            _Client = CreateHttpClient();
        }

        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler { UseProxy = _runscopeSettings.UseProxy };
            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.runscope.com")
            };
            UpdateApiKey(client);

            return client;
        }

        public void ConfigureRunscope()
        {

            var model = new ConfigureViewModel(_runscopeSettings);
            var form = new ConfigureForm(model);

            var result = form.ShowDialog(FiddlerApplication.UI);
            if (result == DialogResult.OK)
            {
                _runscopeSettings.Bucket = model.SelectedBucketKey;
                _runscopeSettings.ApiKey = model.ApiKey;
                _runscopeSettings.UseProxy = model.UseProxy;

                _Client = CreateHttpClient();
            }

        }

        private void UpdateApiKey(HttpClient httpClient)
        {
            if (!String.IsNullOrEmpty(_runscopeSettings.ApiKey))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    _runscopeSettings.ApiKey);
            }
        }

        public void OnBeforeUnload()
        {
            _SendMenuItem.Dispose();
            _ConfigureRunscope.Dispose();
            _Client.Dispose();
        }

        private void ShareSelectedRequests()
        {
            if (!_runscopeSettings.ConfigComplete())
            {
                ConfigureRunscope();
            }

            _hostAdapter.ShowStatus("Connecting to Runscope...");
            SendRequests().ContinueWith(t =>
            {
                if (t.IsCompleted)
                {
                    _hostAdapter.ShowStatus("Completed sharing requests on Runscope");
                }
                else if (t.IsFaulted)
                {
                    if (t.Exception != null)
                    {
                        _hostAdapter.ShowStatus("Failed to share requests on Runscope, because " + t.Exception.Message);
                    }
                    else
                    {
                        _hostAdapter.ShowStatus("Failed to share requests on Runscope");
                    }
                }
            });
           
        }

        private async Task SendRequests()
        {

            Session session = _hostAdapter.SelectedSession;

            var srequest = FiddlerMessageBuilder.CreateRequestFromSession(session);
            var sresponse = FiddlerMessageBuilder.CreateResponseFromSession(session);

            // CreateMessage
            var messagesLink = new MessagesLink();
            var request = messagesLink.BuildPOSTRequest(_runscopeSettings.Bucket, srequest, sresponse);
            var response = await _Client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var messageId = await messagesLink.ParseNewMessageId(response);

                // Share Message
                var sharedMessageLink = new SharedMessageLink();
                var shareRequest = sharedMessageLink.BuildPUTRequest(_runscopeSettings.Bucket, messageId);

                var shareResponse = await _Client.SendAsync(shareRequest);
                var publicurl = await sharedMessageLink.ParsePublicUri(shareResponse);
                if (publicurl != null)
                {
                    // SECURITY: TODO: Do we need to validate that the Absolute URI is HTTP/HTTPS?
                    Process.Start(publicurl.AbsoluteUri);
                }
            }
        }
    }
   
}
