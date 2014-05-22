using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Runscope
{
    public class RunscopeMessage
    {
        public RunscopeRequest Request { get; set; }
        public RunscopeResponse Response { get; set; }
        public Guid UniqueIdentifier { get; set; }

        public HttpContent ToHttpContent()
        {
            var body = new JObject();
            if (Request != null)
            {
                body["request"] = Request.ToJObject();
            }
            if (Response != null)
            {
                body["response"] = Response.ToJObject();
            }
            if (UniqueIdentifier != Guid.Empty)
            {
                body["unique_identifier"] = UniqueIdentifier;
            }
            var content = new StringContent(body.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }

        public static void AddHeaders(HttpHeaders httpHeaders, JObject jheaders)
        {
            foreach (var header in httpHeaders)
            {
                if (header.Value.Count() > 1)
                {
                    string delimiter = _SpaceDelimitedHeaders.Contains(header.Key) ? " " : ", ";
                    jheaders.Add(new JProperty(header.Key, string.Join(delimiter, header.Value)));
                }
                else
                {
                    jheaders.Add(new JProperty(header.Key, header.Value.First()));
                }
            }
        }
        private static readonly HashSet<string> _SpaceDelimitedHeaders =
          new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "User-Agent",
                "Server"
            };
    }

    public class RunscopeRequest
    {
        private JObject _jRequest = new JObject();
        public JObject ToJObject()
        {
            return _jRequest;
        }

        public RunscopeRequest(HttpRequestMessage httpRequest)
        {
            if (httpRequest.RequestUri == null) throw new ArgumentException("Request Uri is required for a Runscope Request");
            _jRequest["method"] = httpRequest.Method.ToString();
            _jRequest["url"] = httpRequest.RequestUri.OriginalString;

            var jheaders = new JObject();
            RunscopeMessage.AddHeaders(httpRequest.Headers,jheaders);
            if (httpRequest.Content != null)
            {
                RunscopeMessage.AddHeaders(httpRequest.Content.Headers, jheaders);
                _jRequest["body"] = httpRequest.Content.ReadAsStringAsync().Result;
            }

            if (jheaders.Properties().Any())
            {
                _jRequest["headers"] = jheaders;
            }
        }

        
    }

    public class RunscopeResponse
    {
        private JObject _jResponse = new JObject();
        
        public JToken ToJObject()
        {
            return _jResponse;
        }

        public RunscopeResponse(HttpResponseMessage httpResponse)
        {
            _jResponse["status"] = (int)httpResponse.StatusCode;

            var jheaders = new JObject();
            RunscopeMessage.AddHeaders(httpResponse.Headers, jheaders);

            if (httpResponse.Content != null)
            {
                RunscopeMessage.AddHeaders(httpResponse.Content.Headers, jheaders);
                _jResponse["body"] = httpResponse.Content.ReadAsStringAsync().Result;
            }
            if (jheaders.Properties().Any())
            {
                _jResponse["headers"] = jheaders;
            }

        }
    }
}
