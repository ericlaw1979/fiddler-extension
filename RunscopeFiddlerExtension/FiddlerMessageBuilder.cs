using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fiddler;

namespace RunscopeFiddlerExtension
{
    public static class FiddlerMessageBuilder
    {
        public static HttpRequestMessage CreateRequestFromSession(Session session)
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

        public static HttpResponseMessage CreateResponseFromSession(Session session)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = (HttpStatusCode)session.responseCode
            };
            var failedHeaders = new List<HTTPHeaderItem>();
            foreach (var header in session.oResponse.headers)
            {
                if (!response.Headers.TryAddWithoutValidation(header.Name, header.Value))
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
