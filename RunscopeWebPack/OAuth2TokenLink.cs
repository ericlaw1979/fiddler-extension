using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RunscopeFiddlerExtension
{
    public class OAuth2TokenLink
    {
        private readonly Dictionary<string, string> _BodyParameters = new Dictionary<string, string>();


        public HttpRequestMessage BuildRequest(Dictionary<string, string> bodyParameters)
        {

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://www.runscope.com/signin/oauth/access_token"),
                Method = HttpMethod.Post,
                Content = new FormUrlEncodedContent(bodyParameters)
            };
            return request;
        }

     
        public static Oauth2Token ParseTokenBody(string tokenBody)
        {
            var jobject = JToken.Parse(tokenBody) as JObject;
            var token = new Oauth2Token();
            foreach (var jprop in jobject.Properties())
            {
                switch (jprop.Name)
                {
                    case "token_type":
                        token.TokenType = (string)jprop.Value;
                        break;
                    case "expires_in":
                        token.ExpiryDate = DateTime.Now + new TimeSpan(0, 0, (int)jprop.Value);
                        break;
                    case "access_token":
                        token.AccessToken = (string)jprop.Value;
                        break;
                    case "refresh_token":
                        token.RefreshToken = (string)jprop.Value;
                        break;
                    case "scope":
                        token.Scope = ((string)jprop.Value).Split(' ');
                        break;
                }
            }

            return token;

        }

        public class OAuth2Error
        {
            public string Error { get; set; }
            public string ErrorDescription { get; set; }
            public string ErrorUri { get; set; }
        }


        public static OAuth2Error ParseErrorBody(string tokenBody)
        {
            var jobject = JToken.Parse(tokenBody) as JObject;
            var token = new OAuth2Error();
            foreach (var jprop in jobject.Properties())
            {
                switch (jprop.Name)
                {
                    case "error":
                        token.Error = (string)jprop.Value;
                        break;
                    case "error_description":
                        token.ErrorDescription = (string)jprop.Value;
                        break;
                    case "error_uri":
                        token.ErrorUri = (string)jprop.Value;
                        break;

                }
            }

            return token;
        }

        public class Oauth2Token
        {
            public string TokenType { get; set; }
            public DateTime ExpiryDate { get; set; }
            public string[] Scope { get; set; }
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }
    }
}
