using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Fiddler;
using Runscope;

namespace RunscopeFiddlerExtension
{
    public class ConfigureViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;
        private List<Bucket> _buckets;
        private string _Bucket;
        private string _ApiKey;
        private bool _UseProxy;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Bucket> Buckets
        {
            get { return _buckets; }
            set
            {
                _buckets = value; 
                OnPropertyChanged();
            }
        }

        public string SelectedBucketKey
        {
            get { return _Bucket; }
            set
            {
                _Bucket = value;
                OnPropertyChanged();
            }
        }

        public string ApiKey
        {
            get { return _ApiKey; }
            set
            {
                _ApiKey = value;
                UpdateAuthHeader();
                RefreshBuckets();
                OnPropertyChanged();
            }
        }

        private void UpdateAuthHeader()
        {
            if (!String.IsNullOrEmpty(_ApiKey))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _ApiKey);
            }
        }

        public bool UseProxy  
        {
            get { return _UseProxy; }
            set
            {
                _UseProxy = value;
                OnPropertyChanged();
            }
        }  

        public ConfigureViewModel(RunscopeSettings runscopeSettings)
        {
            _Bucket = runscopeSettings.Bucket;
            _ApiKey = runscopeSettings.ApiKey;
            _UseProxy = runscopeSettings.UseProxy;

            // Configure form uses it's own HttpClient to enable retreiving a list of buckets using the API key and then allowing the 
            // user to cancel the configure form
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://api.runscope.com")
            };
            UpdateAuthHeader();
            RefreshBuckets();
        }

        private void RefreshBuckets()
        {
            if (!String.IsNullOrEmpty(_ApiKey))
            {
                LoadBuckets();
            }
        }

        public async Task LoadBuckets()
        {
            var bucketsLink = new BucketsLink();
            var response = await _httpClient.SendAsync(bucketsLink.BuildGetRequest());

            var buckets = await bucketsLink.ParseResponse(response);

            Buckets = bucketsLink.ParseBucketList(buckets); 
            
        }
        private string GetBucketKey(string bucketName)
        {
            return Buckets.Where(b => b.Name == bucketName).Select(b => b.Key).FirstOrDefault();
        }

         public void SelectBucket(string bucketName)
        {
            SelectedBucketKey = GetBucketKey(bucketName);
           
        }
         public Bucket SelectedBucket 
         {
             get { return Buckets.FirstOrDefault(b => b.Key == SelectedBucketKey); }

         }

        public void GetApiKey()
        {
            var accessToken = GetAuthToken();

            ApiKey = accessToken;

        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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

            var response = _httpClient.SendAsync(request).Result;
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
    }
}