using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Runscope
{
    public class MessagesLink
    {
        public Uri Target { get; set; }

        public MessagesLink()
        {
            Target = new Uri("buckets/{bucketkey}/messages", UriKind.Relative);
        }
        public HttpRequestMessage BuildPOSTRequest(string bucketKey, HttpRequestMessage request, HttpResponseMessage response)
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(Target.OriginalString.Replace("{bucketkey}",bucketKey), UriKind.Relative)
            };

            var runscopeRequest = new RunscopeRequest(request);
            var runscopeResponse = new RunscopeResponse(response);
            var runscopeMessage = new RunscopeMessage
            {
                Request = runscopeRequest, Response = runscopeResponse
            };

            requestMessage.Content = runscopeMessage.ToHttpContent();
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return requestMessage;
        }

        public static CreateResponse ParseResponseObjectAsync(HttpResponseMessage response)
        {
            var body = JToken.Parse(response.Content.ReadAsStringAsync().Result);

            var createResponse = new CreateResponse {Meta = new CreateMeta()
            {
                ErrorCount = (int)body["meta"]["error_count"],
                SuccessCount = (int)body["meta"]["success_count"],
                WarningCount = (int)body["meta"]["warning_count"],
            }};
            return createResponse;
        }
    }

    public class CreateResponse
    {
        public CreateMeta Meta { get; set; }
    }
    public class CreateMeta
    {
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }
    }
}
