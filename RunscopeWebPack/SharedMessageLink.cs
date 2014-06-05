using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Runscope
{
    public class SharedMessageLink
    {
        public HttpRequestMessage BuildPUTRequest(string bucket, Guid messageId)
        {
            return new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("/buckets/{bucket_key}/shared/{message_id}"
                        .Replace("{bucket_key}",bucket)
                        .Replace("{message_id}",messageId.ToString()), UriKind.Relative)
            };
        }

        public async Task<Uri> ParsePublicUri(HttpResponseMessage response)
        {

            if (response.IsSuccessStatusCode)
            {
                var jObject = JObject.Parse(await response.Content.ReadAsStringAsync());
                return new Uri((string)(jObject["data"]["public_url"]));
            }
            else
            {
                return null;
            }
        }
    }
}
